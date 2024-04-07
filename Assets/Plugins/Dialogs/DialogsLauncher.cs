using System;
using System.Linq;
using Dependencies.ChaserLib.Tasks;
using UnityEngine;

namespace Dependencies.ChaserLib.Dialogs
{
    public class DialogsLauncher : MonoBehaviour, IDialogsLauncher
    {
        [Serializable]
        private class Dialog
        {
            public DialogType Type;
            public GameObject Prefab;
        }

        [SerializeField] private Dialog[] _dialogs;

        private static ServiceLocator.ServiceLocator Locator => ServiceLocator.ServiceLocator.Instance;
        private static ICancellationTokenFactory TokenFactory => Locator.Get<ICancellationTokenFactory>();

        private int _currentSortingOrder;

        public T Show<T>(DialogType dialogType) => Show(dialogType).GetComponent<T>();

        public GameObject Show(DialogType dialogType)
        {
            var prefab = GetPrefab(dialogType);
            var instance = Instantiate(prefab, transform);
            var dialog = instance.GetComponent<DialogBase>();
            var token = TokenFactory.GetDialogClosingToken(dialog.OnClosedSignal);
            dialog.Show();
            dialog.AddCancellationToken(token);
            dialog.GetComponent<Canvas>().sortingOrder = ++_currentSortingOrder;
            return dialog.gameObject;
        }

        private GameObject GetPrefab(DialogType type) => _dialogs.First(d => d.Type == type).Prefab;
    }
}