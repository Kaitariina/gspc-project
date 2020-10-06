using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


[ApiController]
[Route("api/cards/{playerId:Guid}")]
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
    [Route("getone/{cardId:Guid}")]
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

    /*---------- ---------- ---------- ---------- ----------*/

    [HttpGet]
    [Route("getrarest")]
    public Task<Card> GetRarestCard(Guid playerId)
    {

        return repository.GetRarestCard(playerId);
    }

    [HttpGet]
    [Route("getallType")]
    public Task<Card[]> GetAllTypeCards(Guid playerId, CardClassType type)
    {
        return repository.GetAllTypeCards(playerId, type);
    }

    [HttpGet]
    [Route("getType")]
    public Task<Card> GetRarestTypeCard(Guid playerId, CardClassType type)
    {
        return repository.GetRarestTypeCard(playerId, type);
    }

    [HttpGet]
    [Route("getWAtt")]
    public Task<Card> GetCardWHighestAtt(Guid playerId)
    {
        return repository.GetCardWHighestAtt(playerId);
    }

    [HttpGet]
    [Route("getWLowestAtt")]
    public Task<Card> GetCardWLowestAtt(Guid playerId)
    {
        return repository.GetCardWLowestAtt(playerId);
    }

    [HttpGet]
    [Route("getWDef")]
    public Task<Card> GetCardWHighestDef(Guid playerId)
    {
        return repository.GetCardWHighestDef(playerId);
    }
    [HttpGet]
    [Route("getWLowestDef")]
    public Task<Card> GetCardWLowesttDef(Guid playerId)
    {
        return repository.GetCardWLowestDef(playerId);
    }

    [HttpGet]
    [Route("getCommon")]
    public Task<CardClassType> GetMostCommonType(Guid playerId)
    {
        return repository.GetMostCommonType(playerId);
    }

    /*---------- ---------- ---------- ---------- ----------*/

    [HttpOptions]
    public void Options() { }

}