using Common.Application;
using Dependencies.ChaserLib.ServiceLocator;
using Game.Rounds.Events;
using JetBrains.Annotations;
using Plugins.EventHandler;
using Plugins.EventHandler.Handlers;

namespace Game.Timer
{
    [UsedImplicitly]
    internal class TimerHandler : EventHandler
    {
        private static ServiceLocator Locator => ServiceLocator.Instance;
        private static TimerManager TimerManager => Locator.Get<TimerManager>();

        public TimerHandler(IEvent ev) : base(ev)
        {
        }

        public override void Handle()
        {
            if (EventIs<StartRoundEvent>())
            {
                TimerManager.Restart();
            }

            if (EventIs<FinishRoundEvent>() || EventIs<PauseEvent>())
            {
                TimerManager.Pause();
            }

            if (EventIs<ResumeEvent>())
                TimerManager.Resume();
        }
    }
}