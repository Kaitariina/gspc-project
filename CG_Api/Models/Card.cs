using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public enum CardClassType
{
    MAGE,
    HUNTER,
    NINJA
}

public class Card
{
    public Guid Card_Id { get; set; }

    [EnumDataType(typeof(CardClassType))]
    public CardClassType Class { get; set; }

    [Range(1, 4)]
    public int Rarity { get; set; }

    [Range(0, 9)]
    public int Attack { get; set; }

    [Range(1, 9)]
    public int Defence { get; set; }

    public bool Taunt { get; set; }
}

