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
        private static Statistic Statistic => Locator.Get<Statistic>();

        public StatisticHandler(IEvent ev) : base(ev)
        {
        }

        public override void Handle()
        {
            if (EventIs<CorrectPairSelectedEvent>())
            {
                Statistic.IncCorrect();
            }
            else
            {
                Statistic.IncIncorrect();
            }
        }
    }
}