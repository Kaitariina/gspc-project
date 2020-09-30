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
        var filter = Builders<Player>.Filter.Eq(player => player.Player_Id, id);
        return _playerCollection.Find(filter).FirstAsync();
    }

    public async Task<Player[]> GetAll()
    {
        var players = await _playerCollection.Find(new BsonDocument()).ToListAsync();
        return players.ToArray();
    }

    public async Task<Player> Delete(Guid id)
    {
        FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Player_Id, id);
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

        return deck;
    }

    public Task<Deck> DeleteDeck(Guid deckId)
    {
        throw new NotImplementedException();
    }

    public Task<Deck[]> GetDecks(Guid playerId)
    {
        throw new NotImplementedException();
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