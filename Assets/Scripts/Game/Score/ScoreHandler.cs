using Dependencies.ChaserLib.ServiceLocator;
using Game.Cards.Events;
using JetBrains.Annotations;
using Plugins.EventHandler;
using Plugins.EventHandler.Handlers;

namespace Game.Score
{
    [UsedImplicitly]
    internal class ScoreHandler : EventHandler
    {
        private static ServiceLocator Locator => ServiceLocator.Instance;
        private static ScoreCounter ScoreCounter => Locator.Get<ScoreCounter>();
        private static ScoreView ScoreView => Locator.Get<ScoreView>();

        public ScoreHandler(IEvent ev) : base(ev)
        {
        }

        public override void Handle()
        {
            var value = EventIs<CorrectPairSelectedEvent>() ? 50 : -10;
            ScoreCounter.Add(value);
            ScoreView.UpdateScore(ScoreCounter.Get());
        }
    }
}