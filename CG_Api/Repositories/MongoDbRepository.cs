using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

using MongoDB.Bson;
using MongoDB.Driver;


class MongoDbRepository : IRepository
{
    private readonly IMongoCollection<Player> _playerCollection;
    //private readonly IMongoCollection<Card> _cardCollection;
    private readonly IMongoCollection<BsonDocument> _bsonDocumentCollection;
    //private readonly IMongoCollection<BsonDocument> _bsonDocumentCollection2;


    public MongoDbRepository()
    {
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var database = mongoClient.GetDatabase("cardgame");
        _playerCollection = database.GetCollection<Player>("players");
        //_cardCollection = database.GetCollection<Card>("cards");

        _bsonDocumentCollection = database.GetCollection<BsonDocument>("players");
        //_bsonDocumentCollection2 = database.GetCollection<BsonDocument>("cards");
    }

    /*---------- ---------- ---------- ---------- ----------*/

    // Player related
    // Creating a player, finding a player, finding all the players, deleting a player
    //
    public async Task<Player> Create(Player player)
    {
        await _playerCollection.InsertOneAsync(player);
        return player;
    }

    public Task<Player> Get(Guid id)
    {
        var filter = Builders<Player>.Filter.Eq(player => player.Id, id);
        return _playerCollection.Find(filter).FirstAsync();
    }

    public async Task<Player[]> GetAll()
    {
        var players = await _playerCollection.Find(new BsonDocument()).ToListAsync();
        return players.ToArray();
    }

    public async Task<Player> Delete(Guid id)
    {
        FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
        return await _playerCollection.FindOneAndDeleteAsync(filter);
    }

    /*---------- ---------- ---------- ---------- ----------*/

    // Deck related
    // Creating decks, getting decks, deleting & modifying decks
    //
    public async Task<Deck> CreateDeck(Guid playerId)
    {
        Player player = await Get(playerId);

        var deckCreation = new CardMethods();
        var deck = deckCreation.CreateADeck();

        player.DecksOwned.Add(deck);

        var filter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        await _playerCollection.ReplaceOneAsync(filter, player);

        return deck;
    }

    public async Task<Deck> DeleteDeck(Guid deckId, Guid playerId)
    {
        Deck deletedDeck = null;
        Player player = await Get(playerId);

        foreach (var d in player.DecksOwned)
        {
            if (d.Id == d.Id)
            {
                deletedDeck = d;

                var filter_player = Builders<Player>.Filter.Eq(player => player.Id, playerId);
                await _playerCollection.ReplaceOneAsync(filter_player, player);
            }
        }
        return deletedDeck;
    }

    public async Task<Deck> GetDeck(Guid playerId, Guid deckId)
    {
        Deck thisDeck = null;
        Player player = await Get(playerId);

        if (player.DecksOwned.Count > 0)
        {
            foreach (var deck in player.DecksOwned)
            {
                if (deck.Id == deckId)
                {
                    thisDeck = deck;
                }
            }
        }
        return thisDeck;
    }

    public async Task<Deck[]> GetDecks(Guid playerId)
    {
        Player player = await Get(playerId);
        return player.DecksOwned.ToArray();
    }

    public Task<Deck> UpdateDeck(Guid deckId, Card card)
    {
        throw new NotImplementedException();
    }

    /*---------- ---------- ---------- ---------- ----------*/

    // Card related
    // Getting cards, deleting cards
    //
    public Task<Card> GetCard(Guid playerId, Guid cardId)
    {
        throw new NotImplementedException();
    }

    public Task<Card[]> GetAllCardsInDeck(Guid deckId)
    {
        throw new NotImplementedException();
    }
    public Task<Card> DeleteCard(Guid cardId)
    {
        // FilterDefinition<Card> filter = Builders<Card>.Filter.Eq(c => c.Card_Id, cardId);
        // return await _cardCollection.FindOneAndDeleteAsync(filter);
        throw new NotImplementedException();

    }

}