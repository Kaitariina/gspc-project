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
    Task<Deck> UpdateDeck(Guid deckId, Card card);
    Task<Deck> DeleteDeck(Guid deckId);
    Task<Deck[]> GetDecks(Guid playerId);


    Task<Card> GetCard(Guid playerId, Guid cardId);
    Task<Card[]> GetAllCardsInDeck(Guid deckId);
    Task<Card> DeleteCard(Guid cardId);

}