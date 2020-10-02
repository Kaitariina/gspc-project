using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;


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
    [Route("update/{deckId:Guid}")]
    public Task<Deck> UpdateDeck(Guid deckId, Card card)
    {
        return repository.UpdateDeck(deckId, card);
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




    /*---------- ---------- ---------- ---------- ----------*/


    [HttpOptions]
    public void Options() { }
}