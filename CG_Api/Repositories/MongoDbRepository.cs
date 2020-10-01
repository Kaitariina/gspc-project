using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

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
        var filter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        var deckCreation = new CardMethods();

        var deck = deckCreation.CreateADeck();
        deck.Id = Guid.NewGuid();

        var pushDeck = Builders<Player>.Update.Push(player => player.DecksOwned, deck);
        FindOneAndUpdateOptions<Player> options = new FindOneAndUpdateOptions<Player>()
        {
            ReturnDocument = ReturnDocument.After
        };
        Player player = await _playerCollection.FindOneAndUpdateAsync(filter, pushDeck, options);

        //en saanu tätä toimimaan? heitti "sequence has no elements" erroria
        // Player player = await Get(playerId);

        // var deckCreation = new CardMethods();
        // var deck = deckCreation.CreateADeck();

        // player.DecksOwned.Add(deck);

        // var filter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        // await _playerCollection.ReplaceOneAsync(filter, player);

        return deck;
    }

    public async Task<Deck> DeleteDeck(Guid deckId, Guid playerId)
    {
        Deck deletedDeck = null;

        FilterDefinition<Deck> deckFilter = Builders<Deck>.Filter.Eq(deck => deck.Id, deckId);
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.ElemMatch(player => player.DecksOwned, deckFilter);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();

        for (int i = 0; i < player.DecksOwned.Count; i++)
        {
            if (player.DecksOwned[i].Id == deckId)
            {
                deletedDeck = player.DecksOwned[i];

                player.DecksOwned.RemoveAt(i);
                await _playerCollection.ReplaceOneAsync(playerFilter, player);
            }
        }

        return deletedDeck;
    }

    public async Task<Deck> GetDeck(Guid playerId, Guid deckId)
    {
        Deck thisDeck = null;

        var playerFilter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();


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
    public async Task<Card> GetCard(Guid playerId, Guid cardId)
    {
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();

        for (int i = 0; i < player.DecksOwned.Count; i++)
        {
            foreach (var deck in player.DecksOwned)
            {
                foreach (var card in deck.Cards_InDeck)
                {
                    if (card.Id == cardId)
                        return card;
                }
            }
        }
        return null;
    }

    public async Task<Card[]> GetAllCardsInDeck(Guid deckId)
    {
        FilterDefinition<Deck> deckFilter = Builders<Deck>.Filter.Eq(deck => deck.Id, deckId);
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.ElemMatch(player => player.DecksOwned, deckFilter);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();
        List<Card> cardList = new List<Card>();

        for (int i = 0; i < player.DecksOwned.Count; i++)
        {
            foreach (var deck in player.DecksOwned)
            {
                if (deck.Cards_InDeck.Count > 0)
                {
                    foreach (var card in deck.Cards_InDeck)
                    {
                        cardList.Add(card);
                    }
                    return cardList.ToArray();
                }
            }
        }
        return null;
    }
    public async Task<Card> DeleteCard(Guid cardId)
    {
        FilterDefinition<Card> filter = Builders<Card>.Filter.Eq(card => card.Id, cardId);
        FilterDefinition<Deck> deckFilter = Builders<Deck>.Filter.ElemMatch(deck => deck.Cards_InDeck, filter);
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.ElemMatch(player => player.DecksOwned, deckFilter);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();
        int x = 0;

        for (int i = 0; i < player.DecksOwned.Count; i++)
        {
            foreach (var deck in player.DecksOwned)
            {
                if (deck.Cards_InDeck.Count > 0)
                {
                    foreach (var card in deck.Cards_InDeck)
                    {
                        if (card.Id == cardId)
                        {
                            deck.Cards_InDeck.RemoveAt(x);
                            await _playerCollection.ReplaceOneAsync(playerFilter, player);
                            return card;
                        }
                        x++;
                    }
                }
            }
        }
        return null;
    }
}