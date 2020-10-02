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

    [HttpGet]
    [Route("getrarest/{playerId:Guid}")]
    public Task<Card> GetRarestCard(Guid playerId)
    {

        return repository.GetRarestCard(playerId);
    }

    [HttpGet]
    [Route("getallType/{playerId:Guid}")]
    public Task<Card[]> GetAllTypeCards(Guid playerId, CardClassType type)
    {
        return repository.GetAllTypeCards(playerId, type);
    }

    [HttpGet]
    [Route("getType/{playerId:Guid}")]
    public Task<Card> GetRarestTypeCard(Guid playerId, CardClassType type)
    {
        return repository.GetRarestTypeCard(playerId, type);
    }

    [HttpGet]
    [Route("getWAtt/{playerId:Guid}")]
    public Task<Card> GetCardWHighestAtt(Guid playerId)
    {
        return repository.GetCardWHighestAtt(playerId);
    }

    [HttpGet]
    [Route("getWDef/{playerId:Guid}")]
    public Task<Card> GetCardWHighestDef(Guid playerId)
    {
        return repository.GetCardWHighestDef(playerId);
    }

    [HttpGet]
    [Route("getCommon/{playerId:Guid}")]
    public Task<CardClassType> GetMostCommonType(Guid playerId)
    {
        return repository.GetMostCommonType(playerId);
    }

    /*---------- ---------- ---------- ---------- ----------*/

    [HttpOptions]
    public void Options() { }

}