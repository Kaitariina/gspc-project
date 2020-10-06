using System;

public class WorldMethods
{
    public World CreateWorldRuins()
    {
        World world = new World();
        world = Ruins();
        return world;
    }
    public World CreateWorldJungle()
    {
        World world = new World();
        world = Jungle();
        return world;
    }
    public World CreateWorldDesert()
    {
        World world = new World();
        world = Desert();
        return world;
    }
    public World CreateWorldLake()
    {
        World world = new World();
        world = Lake();
        return world;
    }
    public World Ruins()
    {
        World ruins = new World()
        {
            Id = Guid.NewGuid(),
            Difficulty = 1,
            Theme = Theme.RUINS,
            SessionCount = 0
        };
        return ruins;
    }
    public World Jungle()
    {
        World jungle = new World()
        {
            Id = Guid.NewGuid(),
            Difficulty = 2,
            Theme = Theme.JUNGLE,
            SessionCount = 0
        };
        return jungle;
    }
    public World Desert()
    {
        World desert = new World()
        {
            Id = Guid.NewGuid(),
            Difficulty = 3,
            Theme = Theme.DESERT,
            SessionCount = 0
        };
        return desert;
    }
    public World Lake()
    {
        World lake = new World()
        {
            Id = Guid.NewGuid(),
            Difficulty = 4,
            Theme = Theme.LAKE,
            SessionCount = 0
        };
        return lake;
    }
}