using FrostLib.Commands;

namespace Dependencies.ChaserLib.Dialogs.Commands
{
    public class ShowDialogCommand<T> : ICommand<T>
    {
        private static ServiceLocator.ServiceLocator Locator => ServiceLocator.ServiceLocator.Instance;
        private static IDialogsLauncher DialogsLauncher => Locator.Get<IDialogsLauncher>();

        private readonly DialogType _type;

        public ShowDialogCommand(DialogType type) => _type = type;

        public T Execute()
        {
            return DialogsLauncher.Show<T>(_type);
        }
    }
}