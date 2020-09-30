using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;


[ApiController]
[Route("api/players/deck")]
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
    [Route("update")]
    public Task<Deck> UpdateDeck(Guid deckId, Card card)
    {
        return repository.UpdateDeck(deckId, card);
    }

    [HttpDelete]
    [Route("del")]
    public Task<Deck> DeleteDeck(Guid deckId, Guid playerId)
    {
        return repository.DeleteDeck(deckId, playerId);
    }

    [HttpGet]
    [Route("getone")]
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

    [HttpOptions]
    public void Options() { }
}