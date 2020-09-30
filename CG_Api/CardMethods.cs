
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;


public class CardMethods
{
    Random rnd = new Random();

    public Deck CreateADeck()
    {

        var adeck = new Deck();

        for (int i = 0; i < 9; i++)
        {

            int raffle = rnd.Next(1, 4);

            switch (raffle)
            {
                case 1:
                    adeck.Cards_InDeck.Add(CreateMageCard());
                    break;
                case 2:
                    adeck.Cards_InDeck.Add(CreateHunterCard());
                    break;

                case 3:
                    adeck.Cards_InDeck.Add(CreateNinjaCard());
                    break;
            }
        }

        return adeck;
    }

    // // Mage cards have the whole range available in attack and defence
    // // Rarity also varies from common to very rare
    public Card CreateMageCard()
    {
        var card = new Card()
        {
            Card_Id = Guid.NewGuid(),
            Class = CardClassType.MAGE,
            Rarity = rnd.Next(0, 6),            //returns 0 - 6
            Attack = rnd.Next(0, 10),           //returns 0 - 9
            Defence = rnd.Next(1, 10)           //returns 0 - 9
        };

        if (card.Attack == 0)
        {
            card.Taunt = true;
        }

        return card;
    }

    // Hunter cards have more attack, average defence
    // These cards are not that rare
    public Card CreateHunterCard()
    {
        var card = new Card()
        {
            Card_Id = Guid.NewGuid(),
            Class = CardClassType.HUNTER,
            Rarity = rnd.Next(0, 4),            //returns 0 - 3
            Attack = rnd.Next(4, 8),            //returns 4 - 7
            Defence = rnd.Next(3, 6)            //returns 3 - 5
        };

        // If the defence value is lowest,
        // the card is taunt 
        // 
        if (card.Defence == 3)
        {
            card.Taunt = true;
        }

        return card;
    }

    // Ninja cards have high attack, but very low defence
    // These cards are more rare than the others
    // and they never have taunt
    public Card CreateNinjaCard()
    {
        var card = new Card()
        {
            Card_Id = Guid.NewGuid(),
            Class = CardClassType.NINJA,
            Rarity = rnd.Next(3, 6),            //returns 3 - 5
            Attack = rnd.Next(6, 9),            //returns 6 - 8
            Defence = rnd.Next(1, 4),           //returns 1 - 3
            Taunt = false
        };

        return card;
    }
}
