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
    private readonly IMongoCollection<World> _worldCollection;
    //private readonly IMongoCollection<Card> _cardCollection;
    private readonly IMongoCollection<BsonDocument> _bsonDocumentCollection;
    private readonly IMongoCollection<BsonDocument> _bsonDocumentCollectionw;
    //private readonly IMongoCollection<BsonDocument> _bsonDocumentCollection2;


    public MongoDbRepository()
    {
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var database = mongoClient.GetDatabase("cardgame");
        _playerCollection = database.GetCollection<Player>("players");
        _worldCollection = database.GetCollection<World>("worlds");
        //_cardCollection = database.GetCollection<Card>("cards");

        _bsonDocumentCollection = database.GetCollection<BsonDocument>("players");
        _bsonDocumentCollectionw = database.GetCollection<BsonDocument>("worlds");
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


    public async Task<Player> PlayerWHighestRarityCard()
    {
        Player player = null;

        var playersWithRareCards = Builders<Player>.Filter.ElemMatch<Deck>(p => p.DecksOwned, Builders<Deck>
                                    .Filter.ElemMatch(d => d.Cards_InDeck, Builders<Card>.Filter.Eq("Rarity", 5)));

        var players = await _playerCollection.Find(playersWithRareCards).ToListAsync();

        if (players.Count == 0)
        {
            playersWithRareCards = Builders<Player>.Filter.ElemMatch<Deck>(p => p.DecksOwned, Builders<Deck>
                            .Filter.ElemMatch(d => d.Cards_InDeck, Builders<Card>.Filter.Eq("Rarity", 4)));

            players = await _playerCollection.Find(playersWithRareCards).ToListAsync();
        }

        player = players.First();

        return player;

    }

    public async Task<Player> PlayerWHighestRank()
    {
        var players = await _playerCollection.Find(new BsonDocument()).ToListAsync();
        players.OrderByDescending(p => p.Rank);

        return players.First();
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

    public async Task<Deck> UpdateDeck(Guid deckId, Guid cardId)
    {
        int j = 0;

        FilterDefinition<Deck> deckFilter = Builders<Deck>.Filter.Eq(deck => deck.Id, deckId);
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.ElemMatch(player => player.DecksOwned, deckFilter);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();

        var cardCreation = new CardMethods();
        Card newcard = cardCreation.AddANewCard();

        for (int i = 0; i < player.DecksOwned.Count; i++)
        {
            foreach (var deck in player.DecksOwned)
            {
                if (deck.Id == deckId)
                {
                    foreach (var card in deck.Cards_InDeck)
                    {
                        if (card.Id == cardId)
                        {
                            player.DecksOwned[i].Cards_InDeck.RemoveAt(j);
                            player.DecksOwned[i].Cards_InDeck.Add(newcard);

                            player = await _playerCollection.FindOneAndReplaceAsync(playerFilter, player);
                            return player.DecksOwned[i];
                        }
                        j++;
                    }
                }
            }
        }
        return null;
    }


    // More about decks
    // 
    public async Task<Deck> GetDeckWMostOfClass(Guid playerId, CardClassType type)
    {
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();

        var decks = player.DecksOwned;

        Deck deck = null;
        int current = 0;
        int result = 0;

        for (int i = 0; i < player.DecksOwned.Count; i++)
        {
            result = LoopDecksforType(decks[i], type);

            if (result > current)
            {
                deck = decks[i];
                current = result;
            }
        }
        return deck;
    }

    public async Task<Deck> GetDeckWLeastOfClass(Guid playerId, CardClassType type)
    {
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();

        var decks = player.DecksOwned;

        Deck deck = null;
        int current = 0;
        int result = 0;

        for (int i = 0; i < player.DecksOwned.Count; i++)
        {
            result = LoopDecksforType(decks[i], type);

            if (current == 0)
            {
                deck = decks[i];
                current = result;
            }

            if (result < current)
            {
                deck = decks[i];
                current = result;
            }
        }
        return deck;
    }

    // Loop method used to count the amount of x cards in current deck
    //
    public int LoopDecksforType(Deck deck, CardClassType type)
    {
        int amount = 0;

        for (int i = 0; i < deck.Cards_InDeck.Count; i++)
        {
            if (deck.Cards_InDeck[i].Class == type)
            {
                amount = amount + 1;
            }
        }
        return amount;
    }

    public async Task<Deck> GetDeckWMostAttackValue(Guid playerId)
    {
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();

        var decks = player.DecksOwned;

        Deck deck = null;
        int current = 0;

        for (int i = 0; i < player.DecksOwned.Count; i++)
        {
            int result = LoopDecksforAttackValue(decks[i]);

            if (result > current)
            {
                deck = decks[i];
                current = result;
            }
        }

        return deck;
    }

    public async Task<Deck> GetDeckWLeastAttackValue(Guid playerId)
    {
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();

        var decks = player.DecksOwned;

        Deck deck = null;
        int current = 0;

        for (int i = 0; i < player.DecksOwned.Count; i++)
        {
            int result = LoopDecksforAttackValue(decks[i]);

            if (current == 0)
            {
                deck = decks[i];
                current = result;
            }

            if (result < current)
            {
                deck = decks[i];
                current = result;
            }
        }

        return deck;
    }


    public int LoopDecksforAttackValue(Deck deck)
    {
        int amount = 0;

        for (int i = 0; i < deck.Cards_InDeck.Count; i++)
        {
            int add = deck.Cards_InDeck[i].Attack;
            amount = amount + add;
        }

        return amount;
    }

    public async Task<Deck> GetDeckWMostDefenceValue(Guid playerId)
    {
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();

        var decks = player.DecksOwned;

        Deck deck = null;
        int current = 0;

        for (int i = 0; i < player.DecksOwned.Count; i++)
        {
            int result = LoopDecksforDefenceValue(decks[i]);

            if (result > current)
            {
                deck = decks[i];
                current = result;
            }
        }

        return deck;
    }
    public async Task<Deck> GetDeckWLeastDefenceValue(Guid playerId)
    {
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();

        var decks = player.DecksOwned;

        Deck deck = null;
        int current = 0;

        for (int i = 0; i < player.DecksOwned.Count; i++)
        {
            int result = LoopDecksforDefenceValue(decks[i]);

            if (current == 0)
            {
                deck = decks[i];
                current = result;
            }

            if (result < current)
            {
                deck = decks[i];
                current = result;
            }

        }

        return deck;
    }
    public int LoopDecksforDefenceValue(Deck deck)
    {
        int amount = 0;

        for (int i = 0; i < deck.Cards_InDeck.Count; i++)
        {
            int add = deck.Cards_InDeck[i].Defence;
            amount = amount + add;
        }

        return amount;
    }


    public async Task<Deck> GetDeckWMostTauntCards(Guid playerId)
    {
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();

        var decks = player.DecksOwned;

        Deck deck = null;
        int current = 0;

        for (int i = 0; i < player.DecksOwned.Count; i++)
        {
            int result = LoopDecksforTaunt(decks[i]);

            if (result > current)
            {
                deck = decks[i];
                current = result;
            }
        }

        return deck;
    }
    public async Task<Deck> GetDeckWLeastTauntCards(Guid playerId)
    {
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();

        var decks = player.DecksOwned;

        Deck deck = null;
        int current = 0;

        for (int i = 0; i < player.DecksOwned.Count; i++)
        {
            int result = LoopDecksforTaunt(decks[i]);

            if (current == 0)
            {
                deck = decks[i];
                current = result;
            }

            if (result < current)
            {
                deck = decks[i];
                current = result;
            }
        }

        return deck;
    }
    public int LoopDecksforTaunt(Deck deck)
    {
        int amount = 0;

        for (int i = 0; i < deck.Cards_InDeck.Count; i++)
        {
            if (deck.Cards_InDeck[i].Taunt == true)
            {
                amount = amount + 1;
            }
        }
        return amount;
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

    // More about cards
    //
    public async Task<Card> GetRarestCard(Guid playerId)
    {
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();

        var decks = player.DecksOwned;
        List<Card> cards = new List<Card>();

        for (int i = 0; i < decks.Count; i++)
        {
            Deck deck = decks[i];

            for (int j = 0; j < deck.Cards_InDeck.Count; j++)
                cards.Add(deck.Cards_InDeck[j]);
        }

        var sortedCards = cards.OrderByDescending(c => c.Rarity);

        return sortedCards.First();
    }

    public async Task<Card> GetRarestTypeCard(Guid playerId, CardClassType type)
    {
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();
        List<Card> cards = new List<Card>();

        foreach (var deck in player.DecksOwned)
        {
            foreach (var card in deck.Cards_InDeck)
            {
                if (card.Class == type)
                    cards.Add(card);
            }
        }
        var sorted = cards.OrderByDescending(c => c.Rarity);
        return sorted.First();
    }

    public async Task<Card> GetCardWHighestAtt(Guid playerId)
    {
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();
        List<Card> cards = new List<Card>();

        foreach (var deck in player.DecksOwned)
        {
            foreach (var card in deck.Cards_InDeck)
            {
                cards.Add(card);
            }
        }
        var sorted = cards.OrderByDescending(c => c.Attack);
        return sorted.First();
    }

    public async Task<Card> GetCardWHighestDef(Guid playerId)
    {
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();
        List<Card> cards = new List<Card>();

        foreach (var deck in player.DecksOwned)
        {
            foreach (var card in deck.Cards_InDeck)
            {
                cards.Add(card);
            }
        }
        var sorted = cards.OrderByDescending(c => c.Defence);
        return sorted.First();
    }

    public async Task<Card[]> GetAllTypeCards(Guid playerId, CardClassType type)
    {
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();
        List<Card> cards = new List<Card>();

        foreach (var deck in player.DecksOwned)
        {
            foreach (var card in deck.Cards_InDeck)
            {
                if (card.Class == type)
                    cards.Add(card);
            }
        }
        return cards.ToArray();
    }

    public async Task<CardClassType> GetMostCommonType(Guid playerId)
    {
        FilterDefinition<Player> playerFilter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player player = await _playerCollection.Find(playerFilter).FirstAsync();
        List<Card> mages = new List<Card>();
        List<Card> hunters = new List<Card>();
        List<Card> ninjas = new List<Card>();

        foreach (var deck in player.DecksOwned)
        {
            foreach (var card in deck.Cards_InDeck)
            {
                if (card.Class == CardClassType.MAGE)
                    mages.Add(card);
                if (card.Class == CardClassType.HUNTER)
                    hunters.Add(card);
                if (card.Class == CardClassType.NINJA)
                    ninjas.Add(card);
            }
        }


        Console.WriteLine("mages: " + mages.Count + " hunters: " + hunters.Count + " ninjas: " + ninjas.Count);
        if (mages.Count > hunters.Count && mages.Count > ninjas.Count)
            return CardClassType.MAGE;
        if (hunters.Count > mages.Count && hunters.Count > ninjas.Count)
            return CardClassType.HUNTER;
        if (ninjas.Count > mages.Count && ninjas.Count > hunters.Count)
            return CardClassType.NINJA;
        else
        {
            throw new DllNotFoundException();
        }
    }


    /*---------- ---------- ---------- ---------- ----------*/
    // game related
    // create session, worlds and get worlds
    public async Task<GameSession> CreateSession(Guid player1, Guid player2, Guid worldId)
    {
        FilterDefinition<Player> filter1 = Builders<Player>.Filter.Eq(player => player.Id, player1);
        FilterDefinition<Player> filter2 = Builders<Player>.Filter.Eq(player => player.Id, player2);
        Player playerOne = await _playerCollection.Find(filter1).FirstAsync();
        Player playerTwo = await _playerCollection.Find(filter2).FirstAsync();

        FilterDefinition<World> filter = Builders<World>.Filter.Eq(world => world.Id, worldId);
        World sessionWorld = await _worldCollection.Find(filter).FirstAsync();

        var newSession = new GameSession()
        {
            Id = Guid.NewGuid(),
            //muuta ajat loogisemmiksi
            Time_Started = DateTime.Now,
            Time_Finished = DateTime.Now,
            World = sessionWorld,
        };
        if (playerOne.Sessions == null)
            playerOne.Sessions = new List<GameSession>();
        if (playerTwo.Sessions == null)
            playerTwo.Sessions = new List<GameSession>();

        playerOne.Sessions.Add(newSession);
        playerTwo.Sessions.Add(newSession);

        UpdateDefinition<World> updateSession = Builders<World>.Update.Inc(world => world.SessionCount, 1);

        await _playerCollection.ReplaceOneAsync(filter1, playerOne);
        await _playerCollection.ReplaceOneAsync(filter2, playerTwo);
        sessionWorld = await _worldCollection.FindOneAndUpdateAsync(filter, updateSession);

        return newSession;
    }

    public async Task<World[]> CreateWorlds()
    {
        List<World> list = new List<World>();
        WorldMethods worlds = new WorldMethods();

        var ruins = worlds.CreateWorldRuins();
        var jungle = worlds.CreateWorldJungle();
        var desert = worlds.CreateWorldDesert();
        var lake = worlds.CreateWorldLake();

        list.Add(ruins);
        list.Add(jungle);
        list.Add(desert);
        list.Add(lake);
        await _worldCollection.InsertManyAsync(list);

        return list.ToArray();
    }

    public async Task<World[]> GetWorlds()
    {
        var worlds = await _worldCollection.Find(new BsonDocument()).ToListAsync();
        return worlds.ToArray();
    }
}