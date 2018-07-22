using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeCollect.App.Extensions;
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

        public async Task<ContractDto> GetContractWithId(string id)
        {
            return await _client.CreateDocumentQuery<ContractDto>(CollectionLink)
                .SingleOrLog(c => c.Id == id);
        }

        public class Collection<T> where T : Microsoft.Azure.Documents.Document
        {
            private readonly DocumentClient _client;
            private readonly DocumentDb documentDb;

            public Collection(DocumentClient client, DocumentDb documentDb)
            {
                _client = client;
                this.documentDb = documentDb;
            }

            public async Task<T> Get(string id, bool ensureStatusCode = true)
            {
                await documentDb.EnsureDbExists();
                DocumentResponse<T> resp = await _client.ReadDocumentAsync<T>(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return resp.Document;
            }

            public async Task Set(T document, bool ensureStatusCode = true)
            {
                await documentDb.EnsureDbExists();
                ResourceResponse<Microsoft.Azure.Documents.Document> resp = await _client.ReplaceDocumentAsync(document, AccessConditionMatch(document));
            }

            public async Task Create(T document, bool ensureStatusCode = true)
            {
                await documentDb.EnsureDbExists();
                ResourceResponse<Microsoft.Azure.Documents.Document> resp = await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), document, AccessConditionMatch(document));
            }

            public async Task<bool> Exists(string id, bool ensureStatusCode = true)
            {
                await documentDb.EnsureDbExists();
                
                return await _client.CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId))
                        .Exists();
            }

            public async Task<HttpStatusCode> Upsert(T document, bool ensureStatusCode = true)
            {
                await documentDb.EnsureDbExists();
                ResourceResponse<Microsoft.Azure.Documents.Document> resp = await _client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), document, AccessConditionMatch(document));
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
