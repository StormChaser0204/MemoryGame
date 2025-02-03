using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Cards
{
    public class UnlockCardsData
    {
        public readonly Dictionary<string, bool> Cards;

        public UnlockCardsData()
        {
            Cards = new Dictionary<string, bool>
            {
                { "Chesha", true },
                { "Anji", true },
                { "Bast", true },
                { "Cat", true },
                { "CubasovPersik", true },
                { "Felix", true }
            };
        }

        public string[] PickRandom(int amount)
        {
            return Cards
                .Where(c => c.Value)
                .OrderBy(_ => Guid.NewGuid())
                .Take(amount)
                .Select(c => c.Key)
                .ToArray();
        }
    }
}