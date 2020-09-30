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
            Player_Id = Guid.NewGuid(),
            Name = player.Name, //"Player" + (rnd.Next(1, 1000) + rnd.Next(1, 1000)).ToString(),
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


    /*---------- ---------- ---------- ---------- ----------*/

    [HttpOptions]
    public void Options() { }
}