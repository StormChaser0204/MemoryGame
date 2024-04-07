using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Dependencies.ChaserLib.ServiceLocator;
using Game.Cards;
using JetBrains.Annotations;
using Plugins.EventHandler;
using Plugins.EventHandler.Handlers;

namespace Game.Rounds.Handlers
{
    [UsedImplicitly]
    internal class StartRoundHandler : TaskEventHandler
    {
        private static ServiceLocator Locator => ServiceLocator.Instance;
        private static CardHolder CardHolder => Locator.Get<CardHolder>();
        private static Info Info => Locator.Get<Info>();

        public StartRoundHandler(IEvent ev) : base(ev)
        {
        }

        public override async UniTask Handle(CancellationToken cancellationToken = default)
        {
            CardHolder.Dispose();
            var pairsAmount = 6;
            Info.TotalPairsAmount = pairsAmount;
            Info.CurrentPairsAmount = 0;
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