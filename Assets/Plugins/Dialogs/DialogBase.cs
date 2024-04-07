using System.Threading;
using Dependencies.ChaserLib.Tweening.Tweens;
using FrostLib.Signals.impl;
using UnityEngine;

namespace Dependencies.ChaserLib.Dialogs
{
    public class DialogBase : MonoBehaviour
    {
        [SerializeField] private TweenAlpha _alphaTween;
        [SerializeField] private CanvasGroup _canvasGroup;

        public readonly Signal OnClosedSignal = new();

        protected CancellationToken DialogClosingCancellationToken;

        public void AddCancellationToken(CancellationToken token) => DialogClosingCancellationToken = token;

        public virtual void Show()
        {
            _alphaTween.PlayForward();
            SetInteractableState(true);
        }

        public virtual void Hide()
        {
            SetInteractableState(false);
            _alphaTween.PlayReverse();
            _alphaTween.AddOnFinished(() => Destroy(gameObject));
        }

        public virtual void OnDestroy() => OnClosedSignal.Dispatch();

        private void SetInteractableState(bool isOn)
        {
            _canvasGroup.interactable = isOn;
            _canvasGroup.blocksRaycasts = isOn;
        }
    }
}