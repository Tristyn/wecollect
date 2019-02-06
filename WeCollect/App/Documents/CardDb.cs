using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WeCollect.App.Models;
using WeCollect.App.Web3;
using WeCollect.Server.Models;

namespace WeCollect.App.Documents
{
    public class CardDb
    {
        public static CardDb Db;
        private static readonly string DatabaseId = "Main";
        private static readonly Uri DatabaseLink = UriFactory.CreateDatabaseUri(DatabaseId);
        private static readonly string CollectionId = "Main";
        private static readonly Uri CollectionLink = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);

        private readonly DocumentClient _client;
        private readonly ContractArtifacts _contractArtifacts;

        public Collection<ContractDto> Contracts { get; }

        public Collection<CardDto> Cards { get; }

        public Collection<IdCounterDto> EntityIds { get; }

        public Collection<BlockCheckpointDto> BlockCheckpoints { get; }

        private volatile bool _dbExists;

        public CardDb(DocumentClient client, Container container)
        {
            _client = client;

            _contractArtifacts = container.ContractArtifacts;

            Contracts = new Collection<ContractDto>(client, this);
            Cards = new Collection<CardDto>(_client, this);
            EntityIds = new Collection<IdCounterDto>(_client, this);
            BlockCheckpoints = new Collection<BlockCheckpointDto>(_client, this);
            EnsureDbExists().AsTask().Wait();
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
                .SingleOrLog(c => c.id == id);
        }

        public async Task<IEnumerable<ContractDto>> GetAllContracts()
        {
            return await _client.CreateDocumentQuery<ContractDto>(CollectionLink)
                .ToListAsync();
        }

        public async Task<IEnumerable<CardDto>> GetCardSet(string uriName)
        {
            return await _client.CreateDocumentQuery<CardDto>(CollectionLink)
                .Where(card => card.type == nameof(CardDto) && card.uriSet == uriName)
                .ToListAsync();
        }

        public async Task<CardDto> GetCardWithName(string name)
        {
            return await _client.CreateDocumentQuery<CardDto>(CollectionLink)
                .Where(card => card.type == nameof(CardDto) && card.name == name)
                .SingleOrLog();
        }

        public async Task<CardDto> GetCardWithUriName(string uriName)
        {
            return await _client.CreateDocumentQuery<CardDto>(CollectionLink)
                .Where(card => card.type == nameof(CardDto) && card.uriName == uriName)
                .SingleOrLog();
        }

        public async Task<CardDto> GetCardWithCardsContractId(int id)
        {
            return await _client.CreateDocumentQuery<CardDto>(CollectionLink)
                .Where(card => card.type == nameof(CardDto) && card.cardsContractId == id)
                .SingleOrLog();
        }

        public async Task<IEnumerable<CardDto>> GetAllCards()
        {
            return await _client.CreateDocumentQuery<CardDto>(CollectionLink)
                .Where(card => card.type == nameof(CardDto))
                .ToListAsync();
        }
    }
}
