using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dependencies.ChaserLib.Timer;
using Dependencies.ChaserLib.Tweening.Tweens;
using UnityEngine;
using UnityEngine.Events;

namespace Dependencies.ChaserLib.Tweening
{
    [Serializable]
    public class TweenBatcher : IDisposable
    {
        private enum Direction
        {
            Forward,
            Reverse
        }

        [SerializeField] private bool _useScaledTimeNotification = true;
        [Tooltip("Keep -1 to run all tweens")]
        [SerializeField]
        private int _tweenGroup = AllTweensGroup;
        [SerializeField] private GameObject[] _tweenContainers;

        [SerializeField] public UnityEvent OnForwardPlayStarted = new();
        [SerializeField] public UnityEvent OnForwardPlayFinished = new();
        [SerializeField] public UnityEvent OnReversePlayStarted = new();
        [SerializeField] public UnityEvent OnReversePlayFinished = new();

        private TimerBuilder _builder;
        private Tweener[] _tweens;
        private FrostTimer _notificationTimer;
        private Direction _direction = Direction.Forward;

        private const int AllTweensGroup = -1;

        public float LongestDuration { get; private set; }

        public bool IsPlaying
        {
            get
            {
                CollectTweens();
                return _tweens.Any(t => t.enabled);
            }
        }

        public bool CurrentDirectionIsForward => _direction == Direction.Forward;

        public TweenBatcher()
        {
        }

        public TweenBatcher(GameObject[] tweenContainers) => _tweenContainers = tweenContainers;

        public TweenBatcher(GameObject tweenContainer) => _tweenContainers = new[] { tweenContainer };

        public void ResetToBeginning(int group = AllTweensGroup)
        {
            CollectTweens();

            foreach (var tweener in TweensByGroup(group))
            {
                tweener.ResetToBeginningAbsolute();
                tweener.enabled = false;
            }
        }

        public void ResetToEnding(int group = AllTweensGroup)
        {
            CollectTweens();

            foreach (var tweener in TweensByGroup(group))
            {
                tweener.ResetToEndingAbsolute();
                tweener.enabled = false;
            }
        }

        private void CollectTweens()
        {
            if (_tweens != null)
                return;

            _tweens = _tweenContainers.SelectMany(o => o.GetComponents<Tweener>()).ToArray();

            LongestDuration = CalcMaxDuration();
            _builder = new TimerBuilder()
                .Duration(LongestDuration)
                .OnFinish(Notify)
                .AutoStart();

            if (!_useScaledTimeNotification)
                _builder.UseUnscaledTime();
        }

        private float CalcMaxDuration(bool ignoreDelay = false) =>
            _tweens.Length > 0f
                ? _tweens.Max(t => t.Duration + (ignoreDelay ? 0f : t.Delay))
                : 0f;

        public void PlayForward(int group = AllTweensGroup)
        {
            CollectTweens();

            _direction = Direction.Forward;
            foreach (var tween in TweensByGroup(group))
                tween.PlayForward();

            OnForwardPlayStarted.Invoke();
            SetupNotification();
        }

        private IEnumerable<Tweener> TweensByGroup(int group)
        {
            var targetGroup = group == AllTweensGroup ? _tweenGroup : group;
            return targetGroup == AllTweensGroup
                ? _tweens
                : _tweens.Where(t => t.TweenGroup == targetGroup);
        }

        public void PlayReverse(int group = AllTweensGroup, bool ignoreDelay = false)
        {
            CollectTweens();

            _direction = Direction.Reverse;
            foreach (var tween in TweensByGroup(group))
                tween.PlayReverse(ignoreDelay);

            OnReversePlayStarted.Invoke();
            if (ignoreDelay)
                _builder.Duration(CalcMaxDuration(true));

            SetupNotification();

            if (ignoreDelay)
                _builder.Duration(LongestDuration);
        }

        private void SetupNotification()
        {
            _notificationTimer?.StopAndDispose();
            _notificationTimer = _builder.Create();
        }

        private void Notify()
        {
            switch (_direction)
            {
                case Direction.Forward:
                    OnForwardPlayFinished.Invoke();
                    break;
                case Direction.Reverse:
                    OnReversePlayFinished.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_direction), _direction, null);
            }
        }

        public void RemoveDelay()
        {
            CollectTweens();

            foreach (var tween in _tweens)
                tween.Delay = 0f;
        }

        public void Finish(int group = AllTweensGroup)
        {
            CollectTweens();

            foreach (var tween in TweensByGroup(group))
                tween.Finish();
        }

        public void Dispose()
        {
            _notificationTimer?.StopAndDispose();

            OnForwardPlayStarted.RemoveAllListeners();
            OnForwardPlayFinished.RemoveAllListeners();
            OnReversePlayStarted.RemoveAllListeners();
            OnReversePlayFinished.RemoveAllListeners();
        }

        public IEnumerable<T> GetTweens<T>() where T : Tweener
        {
            CollectTweens();
            return _tweens.OfType<T>();
        }

        public IEnumerator GetWait()
        {
            yield return GetWaitInstruction();
        }

        public YieldInstruction GetWaitInstruction() => new WaitForSeconds(LongestDuration);

        public void SetDuration(float duration, int group = AllTweensGroup)
        {
            CollectTweens();

            foreach (var tween in TweensByGroup(group))
                tween.Duration = duration;
        }

        public void SetStyle(Tweener.StyleType newStyle)
        {
            foreach (var tween in _tweens)
                tween.Style = newStyle;
        }

        public void ResetAndPlayForward(int group = AllTweensGroup)
        {
            ResetToBeginning(group);
            PlayForward(group);
        }

        public void SmoothPingPongFinish()
        {
            SetStyle(Tweener.StyleType.Once);
            PlayReverse();
        }

        public void Stop()
        {
            CollectTweens();

            foreach (var tweener in _tweens)
                tweener.Stop();
        }
    }
}