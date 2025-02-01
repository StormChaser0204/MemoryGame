using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Cards;
using Cysharp.Threading.Tasks;
using Dependencies.ChaserLib.ServiceLocator;
using Game.Cards;
using Game.Difficulty;
using JetBrains.Annotations;
using Plugins.EventHandler;
using Plugins.EventHandler.Handlers;
using UnityEngine;

namespace Game.Rounds.Handlers
{
    [UsedImplicitly]
    internal class StartRoundHandler : TaskEventHandler
    {
        private static ServiceLocator Locator => ServiceLocator.Instance;
        private static CardHolder CardHolder => Locator.Get<CardHolder>();
        private static DifficultyManager DifficultyManager => Locator.Get<DifficultyManager>();
        private static RoundStatistic RoundStatistic => Locator.Get<RoundStatistic>();
        private static UnlockCardsData UnlockCardsData => Locator.Get<UnlockCardsData>();

        private const int MinPairsAmount = 2;
        private const int MaxPairsAmount = 12;

        public StartRoundHandler(IEvent ev) : base(ev)
        {
        }

        public override async UniTask Handle(CancellationToken cancellationToken = default)
        {
            CardHolder.Dispose();
            var maxAmount = UnlockCardsData.Cards.Count < MaxPairsAmount
                ? UnlockCardsData.Cards.Count
                : MaxPairsAmount;
            var pairsAmount = Math.Clamp(Mathf.RoundToInt(3 + DifficultyManager.Difficulty),
                MinPairsAmount, maxAmount);

            pairsAmount = pairsAmount > UnlockCardsData.Cards.Count
                ? UnlockCardsData.Cards.Count
                : pairsAmount;

            RoundStatistic.Reset();
            RoundStatistic.TotalPairsAmount = pairsAmount;
            await SetupCards(pairsAmount);
        }

        private static async UniTask SetupCards(int pairsAmount)
        {
            var values = new List<int>();

            for (var i = 0; i < pairsAmount; i++)
            {
                values.Add(i);
                values.Add(i);
            }

            var shufled = values.OrderBy(_ => Guid.NewGuid()).ToList();
            await CardHolder.SetItems(shufled);
        }
    }
}