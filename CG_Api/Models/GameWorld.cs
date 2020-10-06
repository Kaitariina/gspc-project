using System;
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

    [EnumDataType(typeof(Theme))]
    public Theme Theme { get; set; }
    public int SessionCount { get; set; }
}