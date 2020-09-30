using System;
using System.Collections.Generic;

public class GameSession
{
    public Guid Session_Id { get; set; }
    public DateTime Time_Started { get; set; }
    public DateTime Time_Finished { get; set; }
}