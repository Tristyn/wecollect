using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeCollect.App.Models;

namespace WeCollect.App.Documents
{
    public class DocumentDb
    {
        private static readonly string DatabaseId = "Main";
        private static readonly Uri DatabaseLink = UriFactory.CreateDatabaseUri(DatabaseId);
        private static readonly string CollectionId = "Main";
        private static readonly Uri CollectionLink = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);

        private readonly DocumentClient _client;

        public Collection<ContractDto> Contracts { get; }

        private volatile bool _dbExists;

        public DocumentDb(DocumentClient client)
        {
            _client = client;

            Contracts = new Collection<ContractDto>(client, this);
        }

        public async ValueTask EnsureDbExists()
        {
            if (_dbExists)
            {
                return;
            }

            ResourceResponse<Database> resp = await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseId });
            ResourceResponse<DocumentCollection> collResp = await _client.CreateDocumentCollectionIfNotExistsAsync(DatabaseLink, new DocumentCollection { Id = CollectionId });

            _dbExists = true;
        }

        public async Task<ContractDto> GetContractWithName(string name)
        {
            return _client.CreateDocumentQuery<ContractDto>(CollectionLink)
                .Where(c => c.Type == nameof(ContractDto) && c.Name == name)
                .SingleOrDefault();
        }

        public class Collection<T> where T : Document
        {
            private readonly DocumentClient _client;
            private readonly DocumentDb documentDb;

            public Collection(DocumentClient client, DocumentDb documentDb)
            {
                _client = client;
                this.documentDb = documentDb;
            }

            public async Task<(T, HttpStatusCode)> Get(string id, bool ensureStatusCode = true)
            {
                await documentDb.EnsureDbExists();
                DocumentResponse<T> resp = await _client.ReadDocumentAsync<T>(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                if (ensureStatusCode && resp.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new System.Exception();
                }

                return (resp.Document, resp.StatusCode);
            }

            public async Task<HttpStatusCode> Set(T document, bool ensureStatusCode = true)
            {
                await documentDb.EnsureDbExists();
                ResourceResponse<Document> resp = await _client.ReplaceDocumentAsync(document, AccessConditionMatch(document));
                if (ensureStatusCode && resp.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new System.Exception();
                }

                return resp.StatusCode;
            }

            public async Task<HttpStatusCode> Create(T document, bool ensureStatusCode = true)
            {
                await documentDb.EnsureDbExists();
                ResourceResponse<Document> resp = await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), document, AccessConditionMatch(document));
                if (ensureStatusCode && resp.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    throw new System.Exception();
                }

                return resp.StatusCode;
            }

            public async Task<bool> Exists(string id, bool ensureStatusCode = true)
            {
                await documentDb.EnsureDbExists();

                try
                {
                    DocumentResponse<T> resp = await _client.ReadDocumentAsync<T>(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                    return resp.StatusCode == HttpStatusCode.OK;
                }
                catch (DocumentClientException ex)
                {
                    if (ex.IsNotFound())
                    {
                        return false;
                    }

                    throw ex;
                }
            }

            public async Task<HttpStatusCode> Upsert(T document, bool ensureStatusCode = true)
            {
                await documentDb.EnsureDbExists();
                ResourceResponse<Document> resp = await _client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), document, AccessConditionMatch(document));
                if (ensureStatusCode && resp.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new System.Exception();
                }

                return resp.StatusCode;
            }

            private RequestOptions AccessConditionMatch(Resource objectWithEtag)
            {
                return new RequestOptions { AccessCondition = new AccessCondition { Condition = objectWithEtag.ETag, Type = AccessConditionType.IfMatch } };
            }
        }
    }
}

namespace Microsoft.Azure.Documents
{
    public static class DocumentClientExceptionExtensions
    {
        public static bool IsNotFound(this DocumentClientException ex)
        {
            return ex.Error.Code == "NotFound";
        }
    }
}
