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
        decks.Add(await repository.CreateDeck(player.Id));

        List<GameSession> sessions = new List<GameSession>();

        player = new Player()
        {
            Id = Guid.NewGuid(),
            Name = player.Name,
            IsBanned = false,
            CreationTime = DateTime.UtcNow,
            DecksOwned = decks,
            Rank = Rank.NOVICE,
            Sessions = sessions
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

    [HttpGet]
    [Route("getwithmostgames")]
    public async Task<Player> GetPlayerWMostGames()
    {
        return await repository.GetPlayerWMostGames();
    }

    [HttpPost]
    [Route("{id:Guid}/updaterank")]
    public async Task<Player> UpdateRank(Guid id)
    {
        return await repository.UpdateRank(id);
    }

    [HttpPost]
    [Route("{id:Guid}/ban")]
    public async Task<Player> BanPlayer(Guid id)
    {
        return await repository.BanPlayer(id);
    }

    [HttpPost]
    [Route("{id:Guid}/unban")]
    public async Task<Player> UnBanPlayer(Guid id)
    {
        return await repository.UnBanPlayer(id);
    }

    /*---------- ---------- ---------- ---------- ----------*/

    [HttpOptions]
    public void Options() { }
}