using System;

public class GameSession
{
    public Guid Id { get; set; }
    public DateTime Time_Started { get; set; }
    public DateTime Time_Finished { get; set; }

    [WorldValidation]
    public World World { get; set; }
}