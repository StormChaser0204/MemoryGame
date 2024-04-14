using Dependencies.ChaserLib.ServiceLocator;
using Game.Cards.Events;
using JetBrains.Annotations;
using Plugins.EventHandler;
using Plugins.EventHandler.Handlers;

namespace Game.Rounds.Handlers
{
    [UsedImplicitly]
    internal class StatisticHandler : EventHandler
    {
        private static ServiceLocator Locator => ServiceLocator.Instance;
        private static RoundStatistic RoundStatistic => Locator.Get<RoundStatistic>();

        public StatisticHandler(IEvent ev) : base(ev)
        {
        }

        public override void Handle()
        {
            RoundStatistic.IncTriesCount();

            if (EventIs<CorrectPairSelectedEvent>())
            {
                RoundStatistic.IncCorrect();
            }
            else
            {
                RoundStatistic.IncIncorrect();
            }
        }
    }
}