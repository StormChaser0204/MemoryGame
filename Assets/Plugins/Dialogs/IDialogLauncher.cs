using UnityEngine;

namespace Dependencies.ChaserLib.Dialogs
{
    public interface IDialogsLauncher
    {
        T Show<T>(DialogType dialogType);
        GameObject Show(DialogType dialogType);
    }
}