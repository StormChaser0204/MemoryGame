using System;
using Cysharp.Threading.Tasks;
using Dependencies.ChaserLib.Tweening.Tweens;
using FrostLib.Signals.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Cards
{
    internal class Card : MonoBehaviour, IDisposable
    {
        [SerializeField] private Image _value;
        [SerializeField] private TweenRectTransformSize _coverTween;
        [SerializeField] private TweenScale _scaleTween;

        public readonly Signal<Card> OnSelected = new();

        private int _id;

        public void Init(int id, Sprite sprite)
        {
            _value.sprite = sprite;
            _id = id;
        }

        public int GetId() => _id;

        public void Select() => OnSelected.Dispatch(this);

        public void Hide()
        {
            _coverTween.Play(true);
        }

        public async UniTask Show()
        {
            _coverTween.ResetToEnding();
            _coverTween.Play(false);
            await UniTask.Delay(TimeSpan.FromSeconds(_coverTween.Duration));
        }

        public void ShowCard()
        {
            _scaleTween.ResetToBeginningAbsolute();
            _scaleTween.PlayForward();
        }

        public void RemoveCard()
        {
            _scaleTween.PlayReverse();
        }

        public void Dispose()
        {
            OnSelected.ClearListeners();
            Destroy(gameObject);
        }
    }
}