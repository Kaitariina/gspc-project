using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;


[ApiController]
[Route("api/players")]
public class PlayersController
{
    private readonly ILogger<PlayersController> logger;
    private readonly IRepository repository;

    public PlayersController(ILogger<PlayersController> alogger, IRepository arepository)
    {
        logger = alogger;
        repository = arepository;
    }

    /*---------- ---------- ---------- ---------- ----------*/

    [HttpPost]
    [Route("create")]
    public async Task<Player> Create([FromBody] Player player)
    {
        Random rnd = new Random();
        List<Deck> decks = new List<Deck>();

        player = new Player()
        {
            Id = Guid.NewGuid(),
            Name = player.Name,
            IsBanned = false,
            CreationTime = DateTime.UtcNow,
            DecksOwned = decks,
            Rank = Rank.AMATEUR
        };

        logger.LogInformation("Player created: " + player.Name);

        await repository.Create(player);

        return player;
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<Player> Get(Guid id)
    {
        return await repository.Get(id);
    }

    [HttpGet]
    [Route("getall")]
    public Task<Player[]> GetAll()
    {
        return repository.GetAll();
    }
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<Player> Delete(Guid id)
    {
        logger.LogInformation("Player with id: " + id + "deleted");

        return await repository.Delete(id);
    }

    [HttpGet]
    [Route("getwithrarecard")]
    public async Task<Player> PlayerWHighestRarityCard()
    {
        return await repository.PlayerWHighestRarityCard();
    }

    [HttpGet]
    [Route("getwithbestrank")]
    public async Task<Player> PlayerWHighestRank()
    {
        return await repository.PlayerWHighestRank();
    }


    //game session
    [HttpPost]
    [Route("createSession/{player1:Guid}/{player2:Guid}/{worldId:Guid}")]
    public async Task<GameSession> CreateSession(Guid player1, Guid player2, Guid worldId)
    {
        return await repository.CreateSession(player1, player2, worldId);
    }

    //game world
    [HttpPost]
    [Route("createWorlds")]
    public async Task<World[]> CreateWorlds()
    {
        return await repository.CreateWorlds();
    }
    [HttpGet]
    [Route("getWorlds")]
    public async Task<World[]> GetWorlds()
    {
        return await repository.GetWorlds();
    }
    /*---------- ---------- ---------- ---------- ----------*/

    [HttpOptions]
    public void Options() { }
}