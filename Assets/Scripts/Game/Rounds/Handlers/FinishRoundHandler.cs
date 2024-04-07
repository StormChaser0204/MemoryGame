using Dependencies.ChaserLib.Dialogs;
using Dependencies.ChaserLib.ServiceLocator;
using JetBrains.Annotations;
using Plugins.EventHandler;
using Plugins.EventHandler.Handlers;

namespace Game.Rounds.Handlers
{
    [UsedImplicitly]
    internal class FinishRoundHandler : EventHandler
    {
        private static ServiceLocator Locator => ServiceLocator.Instance;
        private static IDialogsLauncher DialogsLauncher => Locator.Get<IDialogsLauncher>();

        public FinishRoundHandler(IEvent ev) : base(ev)
        {
        }

        public override void Handle()
        {
            DialogsLauncher.Show(DialogType.FinnishRound);
        }
    }
}