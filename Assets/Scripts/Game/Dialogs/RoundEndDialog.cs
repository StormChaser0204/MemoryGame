using Dependencies.ChaserLib.Dialogs;
using Dependencies.ChaserLib.ServiceLocator;
using Game.Rounds.Events;
using Plugins.EventDispatching.Dispatcher;

namespace Game.Dialogs
{
    internal class RoundEndDialog : DialogBase
    {
        private static ServiceLocator Locator => ServiceLocator.Instance;
        private static IEventDispatcher Dispatcher => Locator.Get<IEventDispatcher>();

        public void LoadNextRound()
        {
            Dispatcher.Raise(new StartRoundEvent());
            Hide();
        }
    }
}