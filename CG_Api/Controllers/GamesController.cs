using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;


[ApiController]
[Route("api/players/games")]
public class GamesController
{
    private readonly ILogger<PlayersController> logger;
    private readonly IRepository repository;

    public GamesController(ILogger<PlayersController> alogger, IRepository arepository)
    {
        logger = alogger;
        repository = arepository;
    }

    /*---------- ---------- ---------- ---------- ----------*/

    //game session
    [HttpPost]
    [Route("createSession/{player1:Guid}/{player2:Guid}")]
    public async Task<GameSession> CreateSession(Guid player1, Guid player2, World world)
    {
        Random rnd = new Random();
        World[] array = await repository.GetWorlds();
        List<World> worlds = new List<World>();
        foreach (var i in array)
        {
            worlds.Add(i);
        }

        if (worlds.Count == 0)
            world = null;
        else
            world = worlds[rnd.Next(0, 4)];

        return await repository.CreateSession(player1, player2, world);
    }

    [HttpGet]
    [Route("getSessions")]
    public async Task<GameSession[]> GetSessions()
    {
        return await repository.GetSessions();
    }

    [HttpGet]
    [Route("longest")]
    public async Task<GameSession> GetLongestSession()
    {
        return await repository.GetLongestSession();
    }

    [HttpGet]
    [Route("shortest")]
    public async Task<GameSession> GetShortestSession()
    {
        return await repository.GetShortestSession();
    }

    /*---------- ---------- ---------- ---------- ----------*/

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

    [HttpGet]
    [Route("mostPlayed")]
    public async Task<World> GetMostPlayed()
    {
        return await repository.GetMostPlayed();
    }

    [HttpGet]
    [Route("leastPlayed")]
    public async Task<World> GetLeastPlayed()
    {
        return await repository.GetLeastPlayed();
    }

    [HttpGet]
    [Route("mostDifficult")]
    public async Task<World> GetMostDifficult()
    {
        return await repository.GetMostDifficult();
    }

    /*---------- ---------- ---------- ---------- ----------*/

    [HttpOptions]
    public void Options() { }
}