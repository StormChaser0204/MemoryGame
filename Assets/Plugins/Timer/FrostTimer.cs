using System;
using System.Collections;
using UnityEngine;

namespace Dependencies.ChaserLib.Timer
{
    public class FrostTimer
    {
        [Flags]
        private enum State
        {
            NotStarted = 0,
            Started = 1,
            Running = 2,
            Completed = 4,
            ShouldBeDisposed = 8
        }

        public bool IsRunning => _state.HasFlag(State.Running);
        public bool IsCompleted => _state.HasFlag(State.Completed);
        public bool IsStarted => _state.HasFlag(State.Started);
        public bool ShouldBeDisposed => _state.HasFlag(State.ShouldBeDisposed);
        public bool IsPaused => _state.HasFlag(State.Started) && !_state.HasFlag(State.Running);

        public float TimeLeft { get; private set; }
        public float TimePassed { get; private set; }
        public float Progress => TimePassed / Duration;
        public float Duration { get; private set; }

        public readonly bool DisposedOnSceneUnload;

        public bool UseScaledTime = true;
        public bool IsLooped;
        public float TimeScale = 1f;

        public Action OnStart;
        public Action<FrostTimer> OnTickTimer;
        public Action OnTick;
        public Action OnComplete;
        public Action OnStop;

        private State _state = State.NotStarted;

        public FrostTimer(Action onFinish, float duraiton, bool disposedOnSceneUnload = false)
        {
            OnComplete += onFinish;
            Duration = TimeLeft = duraiton;
            DisposedOnSceneUnload = disposedOnSceneUnload;
        }

        public void DisposeOnFinish() => OnComplete += StopAndDispose;

        public void StopAndDispose()
        {
            Stop();
            Dispose();
        }

        public void Stop()
        {
            RemoveFlagIfExists(State.Started);
            RemoveFlagIfExists(State.Running);

            TimePassed = 0;

            OnStop?.Invoke();
        }

        private void Dispose()
        {
            _state |= State.ShouldBeDisposed;

            OnStart = null;
            OnTick = null;
            OnTickTimer = null;
            OnComplete = null;
            OnStop = null;
        }

        public void Start() => Restart();

        public void Restart() => Start(Duration);

        public void Start(float duration)
        {
            ResetDuration(duration);
            _state = State.Started | State.Running;
            OnStart?.Invoke();
        }

        public void ResetDuration(float duration)
        {
            Duration = TimeLeft = duration;
            TimePassed = 0;
        }

        public void Tick()
        {
            if (!IsRunning)
                return;

            var deltaTime = (UseScaledTime ? Time.deltaTime : Time.unscaledDeltaTime) * TimeScale;
            TimePassed = Mathf.Min(Duration, TimePassed + deltaTime);
            TimeLeft = Mathf.Max(0, TimeLeft - deltaTime);

            OnTick?.Invoke();
            OnTickTimer?.Invoke(this);

            if (TimeLeft > 0)
                return;

            if (IsLooped && IsRunning)
                Restart();
            else
                _state = State.Completed;

            // In the callback method we may change state of the timer. Thus it should be called last 
            OnComplete?.Invoke();
        }

        public void Pause() => RemoveFlagIfExists(State.Running);

        public void Resume() => _state |= State.Running;

        private void RemoveFlagIfExists(State flag)
        {
            if (_state.HasFlag(flag))
                _state ^= flag;
        }

        public IEnumerator GetWait()
        {
            if (UseScaledTime)
                yield return new WaitForSeconds(Duration);
            else
                yield return new WaitForSecondsRealtime(Duration);
        }
    }
}