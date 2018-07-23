using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
namespace WeCollect.App.Documents
{

    public class Collection<T> where T : WeCollect.App.Models.Document
    {
        private static readonly string DatabaseId = "Main";
        private static readonly Uri DatabaseLink = UriFactory.CreateDatabaseUri(DatabaseId);
        private static readonly string CollectionId = "Main";
        private static readonly Uri CollectionLink = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);

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
            
            var resp = await _client.ReplaceDocumentAsync(CollectionLink,document, AccessConditionMatch(document));
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

        private RequestOptions AccessConditionMatch(T objectWithEtag)
        {
            return new RequestOptions { AccessCondition = new AccessCondition { Condition = objectWithEtag.ETag, Type = AccessConditionType.IfMatch } };
        }
    }
}
