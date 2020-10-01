using System;
using System.Threading.Tasks;

public interface IRepository
{
    // Player related
    //
    Task<Player> Get(Guid id);
    Task<Player[]> GetAll();
    Task<Player> Create(Player player);
    Task<Player> Delete(Guid id);

    // Decks, cards
    //
    Task<Deck> CreateDeck(Guid playerId);
    Task<Deck> DeleteDeck(Guid deckId, Guid playerId);
    Task<Deck> GetDeck(Guid playerId, Guid deckId);
    Task<Deck[]> GetDecks(Guid playerId);
    Task<Deck> UpdateDeck(Guid deckId, Card card);


    Task<Card> GetCard(Guid playerId, Guid cardId);
    Task<Card[]> GetAllCardsInDeck(Guid deckId);
    Task<Card> DeleteCard(Guid cardId);


    Task<GameSession> CreateSession(Guid player1, Guid player2, Guid worldId);
    public Task<World[]> CreateWorlds();
    public Task<World[]> GetWorlds();
}