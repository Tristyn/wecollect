using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeCollect.App.Models;
using WeCollect.App.Web3;

namespace WeCollect.App.Documents
{
    public class CardDocumentDb
    {
        private static readonly string DatabaseId = "Main";
        private static readonly Uri DatabaseLink = UriFactory.CreateDatabaseUri(DatabaseId);
        private static readonly string CollectionId = "Main";
        private static readonly Uri CollectionLink = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);

        private readonly DocumentClient _client;
        private readonly ContractArtifacts _contractArtifacts;

        public Collection<ContractDto> Contracts { get; }

        public Collection<CardDto> Cards { get; }

        public Collection<Models.BlockCheckpointDto> BlockCheckpoints { get; }

        private volatile bool _dbExists;
        
        public CardDocumentDb(DocumentClient client, Container container)
        {
            _client = client;
            
            _contractArtifacts = container.ContractArtifacts;
            
            Contracts = new Collection<ContractDto>(client, this);
            Cards = new Collection<CardDto>(_client, this);
            BlockCheckpoints = new Collection<Models.BlockCheckpointDto>(_client, this);
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

        public async Task<IEnumerable<ContractDto>> GetAllContracts()
        {
            return await _client.CreateDocumentQuery<ContractDto>(CollectionLink)
                .ToListAsync();
        }

        public Task<IEnumerable<CardDto>> GetCardSet(string name)
        {
            return _client.CreateDocumentQuery<CardDto>(CollectionLink)
                .Where(card => card.Set == name)
                .ToListAsync();
        }
    }
}
