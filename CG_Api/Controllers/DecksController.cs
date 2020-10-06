using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


[ApiController]
[Route("api/players/{playerId:Guid}/deck")]
public class DecksController
{
    private readonly ILogger<DecksController> logger;
    private readonly IRepository repository;

    public DecksController(ILogger<DecksController> alogger, IRepository arepository)
    {
        logger = alogger;
        repository = arepository;
    }

    /*---------- ---------- ---------- ---------- ----------*/

    [HttpPost]
    [Route("create")]
    public Task<Deck> CreateDeck(Guid playerId)
    {
        return repository.CreateDeck(playerId);
    }

    [HttpPost]
    [Route("update/{deckId:Guid}/{cardId:Guid}")]
    public Task<Deck> UpdateDeck(Guid deckId, Guid cardId)
    {
        return repository.UpdateDeck(deckId, cardId);
    }

    [HttpDelete]
    [Route("del/{deckId:Guid}")]
    public Task<Deck> DeleteDeck(Guid deckId, Guid playerId)
    {
        return repository.DeleteDeck(deckId, playerId);
    }

    [HttpGet]
    [Route("getone/{deckId:Guid}")]
    public Task<Deck> GetDeck(Guid playerId, Guid deckId)
    {
        return repository.GetDeck(playerId, deckId);
    }

    [HttpGet]
    [Route("getall")]
    public Task<Deck[]> GetDecks(Guid playerId)
    {
        return repository.GetDecks(playerId);
    }

    [HttpGet]
    [Route("getWCard/{cardId:Guid}")]
    public Task<Deck> GetDeckWCard(Guid cardId)
    {
        return repository.GetDeckWCard(cardId);
    }

    /*---------- ---------- ---------- ---------- ----------*/

    // Postman: write only a single number in the body field
    // 0 = MAGE, 1 = HUNTER, 2 = NINJA
    [HttpGet]
    [Route("getleastoftype")]
    public Task<Deck> GetDeckWLeastOfClass(Guid playerId, [FromBody] CardClassType type)
    {
        return repository.GetDeckWLeastOfClass(playerId, type);
    }

    // Postman: write only a single number in the body field
    // 0 = MAGE, 1 = HUNTER, 2 = NINJA
    [HttpGet]
    [Route("getmostoftype")]
    public Task<Deck> GetDeckWMostOfClass(Guid playerId, [FromBody] CardClassType type)
    {
        return repository.GetDeckWMostOfClass(playerId, type);
    }


    [HttpGet]
    [Route("getmostatk")]
    public Task<Deck> GetDeckWMostAttackValue(Guid playerId)
    {
        return repository.GetDeckWMostAttackValue(playerId);
    }

    [HttpGet]
    [Route("getleastatk")]
    public Task<Deck> GetDeckWLeastAttackValue(Guid playerId)
    {
        return repository.GetDeckWLeastAttackValue(playerId);
    }


    [HttpGet]
    [Route("getmostdef")]
    public Task<Deck> GetDeckWMostDefencekValue(Guid playerId)
    {
        return repository.GetDeckWMostDefenceValue(playerId);
    }

    [HttpGet]
    [Route("getleastdef")]
    public Task<Deck> GetDeckWLeastDefenceValue(Guid playerId)
    {
        return repository.GetDeckWLeastDefenceValue(playerId);
    }

    [HttpGet]
    [Route("getmosttaunts")]
    public Task<Deck> GetDeckWMostTauntCards(Guid playerId)
    {
        return repository.GetDeckWMostTauntCards(playerId);

    }


    [HttpGet]
    [Route("getleasttaunts")]
    public Task<Deck> GetDeckWLeastTauntCards(Guid playerId)
    {
        return repository.GetDeckWLeastTauntCards(playerId);

    }

    /*---------- ---------- ---------- ---------- ----------*/


    [HttpOptions]
    public void Options() { }
}