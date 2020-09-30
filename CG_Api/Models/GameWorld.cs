using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public enum Theme
{
    RUINS,
    JUNGLE,
    DESERT,
    LAKE
}

public class World
{
    public Guid Id { get; set; }

    [Range(0, 5)]
    public int Difficulty { get; set; }
    public Theme Theme { get; set; }
}