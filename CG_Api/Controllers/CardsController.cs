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
    [Route("getone/{playerId:Guid}/{cardId:Guid}")]
    public Task<Card> GetCard(Guid playerId, Guid cardId)
    {
        return repository.GetCard(playerId, cardId);
    }

    [HttpGet]
    [Route("getall/{deckId:Guid}")]
    public Task<Card[]> GetAllCardsInDeck(Guid deckId)
    {
        return repository.GetAllCardsInDeck(deckId);
    }

    [HttpDelete]
    [Route("delete/{cardId:Guid}")]
    public Task<Card> DeleteCard(Guid cardId)
    {
        return repository.DeleteCard(cardId);
    }

    /*---------- ---------- ---------- ---------- ----------*/

    [HttpOptions]
    public void Options() { }

}