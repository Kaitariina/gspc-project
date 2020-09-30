using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Deck
{
    public Guid Deck_Id { get; set; }
    public List<Card> Cards_InDeck { get; set; }

}
