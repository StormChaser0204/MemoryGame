using System;

namespace Dependencies.ChaserLib.Timer
{
    public class TimerBuilder
    {
        private Action _onFinish;
        private float _duration;
        private bool _disposeOnFinish = true;
        private bool _isLooped;
        private bool _useScaledTime = true;
        private bool _regularUpdate = true;
        private bool _autoStart;
        private bool _transferOwnership = true;
        private Action _onStart;
        private Action _onTick;
        private Action<FrostTimer> _onTickTimer;
        private Action _onStop;
        private bool _disposeOnSceneUnload;

        public TimerBuilder OnStart(Action onStart)
        {
            _onStart = onStart;
            return this;
        }

        public TimerBuilder OnTick(Action onTick)
        {
            _onTick = onTick;
            return this;
        }

        public TimerBuilder OnTick(Action<FrostTimer> onTick)
        {
            _onTickTimer = onTick;
            return this;
        }

        public TimerBuilder OnFinish(Action onFinish)
        {
            _onFinish = onFinish;
            return this;
        }

        public TimerBuilder OnStop(Action onStop)
        {
            _onStop = onStop;
            return this;
        }

        public TimerBuilder Duration(float duration)
        {
            _duration = duration;
            return this;
        }

        public TimerBuilder KeepAlive()
        {
            _disposeOnFinish = false;
            return this;
        }

        public TimerBuilder Loop()
        {
            _isLooped = true;
            return this;
        }

        public TimerBuilder UseUnscaledTime()
        {
            _useScaledTime = false;
            return this;
        }

        public TimerBuilder UseFixedUpdate()
        {
            _regularUpdate = false;
            return this;
        }

        public TimerBuilder AutoStart()
        {
            _autoStart = true;
            return this;
        }

        public TimerBuilder KeepOwnership()
        {
            _transferOwnership = false;
            return this;
        }

        public TimerBuilder DisposeOnSceneUnload()
        {
            _disposeOnSceneUnload = true;
            return this;
        }

        public FrostTimer Create()
        {
            var timer = new FrostTimer(_onFinish, _duration, _disposeOnSceneUnload)
            {
                IsLooped = _isLooped,
                UseScaledTime = _useScaledTime,
                OnStart = _onStart,
                OnTick = _onTick,
                OnTickTimer = _onTickTimer,
                OnStop = _onStop
            };

            HandleDispose(timer);
            HandleTransfer(timer);
            HandleAutostart(timer);

            return timer;
        }

        private void HandleDispose(FrostTimer timer)
        {
            if (!_disposeOnFinish)
                return;

            timer.DisposeOnFinish();
        }

        private void HandleTransfer(FrostTimer timer)
        {
            if (!_transferOwnership)
                return;

            var manager = TimerManager.Instance;
            if (_regularUpdate)
                manager.Add(timer);
            else
                manager.AddFixed(timer);
        }

        private void HandleAutostart(FrostTimer timer)
        {
            if (!_autoStart)
                return;

            timer.Start(_duration);
        }
    }
}