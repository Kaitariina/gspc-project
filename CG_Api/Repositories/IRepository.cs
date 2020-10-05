using System;
using System.Threading.Tasks;

public interface IRepository
{
    // Players
    //
    Task<Player> Get(Guid id);
    Task<Player[]> GetAll();
    Task<Player> Create(Player player);
    Task<Player> Delete(Guid id);

    Task<Player> PlayerWHighestRarityCard();
    Task<Player> PlayerWHighestRank();
    Task<Player> GetPlayerWMostGames();
    Task<Player> UpdateRank(Guid playerId);
    Task<Player> BanPlayer(Guid playerId);
    Task<Player> UnBanPlayer(Guid playerId);


    // Decks
    //
    Task<Deck> CreateDeck(Guid playerId);
    Task<Deck> DeleteDeck(Guid deckId, Guid playerId);
    Task<Deck> GetDeck(Guid playerId, Guid deckId);
    Task<Deck[]> GetDecks(Guid playerId);
    Task<Deck> UpdateDeck(Guid deckId, Guid cardId);

    Task<Deck> GetDeckWMostAttackValue(Guid playerId);
    Task<Deck> GetDeckWLeastAttackValue(Guid playerId);
    Task<Deck> GetDeckWMostDefenceValue(Guid playerId);
    Task<Deck> GetDeckWLeastDefenceValue(Guid playerId);
    Task<Deck> GetDeckWMostTauntCards(Guid playerId);
    Task<Deck> GetDeckWLeastTauntCards(Guid playerId);
    Task<Deck> GetDeckWLeastOfClass(Guid playerId, CardClassType type);
    Task<Deck> GetDeckWMostOfClass(Guid playerId, CardClassType type);


    // Cards
    //
    Task<Card> GetCard(Guid playerId, Guid cardId);
    Task<Card[]> GetAllCardsInDeck(Guid deckId);
    Task<Card> DeleteCard(Guid cardId);

    Task<Card> GetRarestCard(Guid playerId);
    Task<Card> GetRarestTypeCard(Guid playerId, CardClassType type);
    Task<CardClassType> GetMostCommonType(Guid playerId);
    Task<Card> GetCardWHighestAtt(Guid playerId);
    Task<Card> GetCardWHighestDef(Guid playerId);
    Task<Card[]> GetAllTypeCards(Guid playerId, CardClassType type);


    // Sessions & Worlds
    //
    Task<GameSession> CreateSession(Guid player1, Guid player2, Guid worldId);

    public Task<GameSession> GetLongestSession();
    public Task<GameSession> GetShortestSession();
    public Task<GameSession[]> GetSessions();

    public Task<World[]> CreateWorlds();
    public Task<World[]> GetWorlds();

    public Task<World> GetMostPlayed();
    public Task<World> GetLeastPlayed();
    public Task<World> GetMostDifficult();
}