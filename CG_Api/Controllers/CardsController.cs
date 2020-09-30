using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;


[ApiController]
[Route("api/cards")]
public class CardsController
{
    private readonly ILogger<CardsController> logger;
    private readonly IRepository repository;

    public CardsController(ILogger<CardsController> alogger, IRepository arepository)
    {
        logger = alogger;
        repository = arepository;
    }

    /*---------- ---------- ---------- ---------- ----------*/

    [HttpGet]
    [Route("getone")]
    public Task<Card> GetCard(Guid playerId, Guid cardId)
    {
        return repository.GetCard(playerId, cardId);
    }

    [HttpGet]
    [Route("getall")]
    public Task<Card[]> GetAllCardsInDeck(Guid deckId)
    {
        return repository.GetAllCardsInDeck(deckId);
    }

    [HttpGet]
    [Route("delete")]
    public Task<Card> DeleteCard(Guid cardId)
    {
        return repository.DeleteCard(cardId);
    }

    /*---------- ---------- ---------- ---------- ----------*/

    [HttpOptions]
    public void Options() { }

}