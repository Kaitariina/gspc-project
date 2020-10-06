using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Enumerator for player ranks, Start at 1, 0 is uninitialized
public enum Rank
{
    NOVICE = 1,
    AMATEUR,
    PROFESSIONAL
}

public class Player
{
    public Guid Id { get; set; }
    [StringLength(10, ErrorMessage = "Name too long")]
    public string Name { get; set; }
    public bool IsBanned { get; set; }
    public DateTime CreationTime { get; set; }
    public Rank Rank { get; set; }
    public List<Deck> DecksOwned { get; set; }
    public List<GameSession> Sessions { get; set; }
}