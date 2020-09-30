
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
        var deckSize = rnd.Next(5, 10);


        for (int i = 0; i < deckSize; i++)
        {

            int raffle = rnd.Next(1, 3);

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
            Rarity = rnd.Next(0, 5),
            Attack = rnd.Next(0, 9),
            Defence = rnd.Next(1, 9)
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
            Rarity = rnd.Next(0, 3),
            Attack = rnd.Next(4, 7),
            Defence = rnd.Next(3, 5)
        };

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
            Rarity = rnd.Next(3, 5),
            Attack = rnd.Next(6, 8),
            Defence = rnd.Next(1, 3),
            Taunt = false
        };

        return card;
    }
}
