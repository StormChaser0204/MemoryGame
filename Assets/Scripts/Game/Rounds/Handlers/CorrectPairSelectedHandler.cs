using Dependencies.ChaserLib.ServiceLocator;
using Game.Rounds.Events;
using JetBrains.Annotations;
using Plugins.EventDispatching.Dispatcher;
using Plugins.EventHandler;
using Plugins.EventHandler.Handlers;

namespace Game.Rounds.Handlers
{
    [UsedImplicitly]
    internal class CorrectPairSelectedHandler : EventHandler
    {
        private static ServiceLocator Locator => ServiceLocator.Instance;
        private static RoundStatistic RoundStatistic => Locator.Get<RoundStatistic>();
        private static IEventDispatcher Dispatcher => Locator.Get<IEventDispatcher>();

        public CorrectPairSelectedHandler(IEvent ev) : base(ev)
        {
        }

        public override void Handle()
        {
            RoundStatistic.CurrentPairsAmount++;
            if (RoundStatistic.CurrentPairsAmount != RoundStatistic.TotalPairsAmount)
                return;

            Dispatcher.Raise(new FinishRoundEvent());
        }
    }
}