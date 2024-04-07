using System;
using UnityEngine;
using UnityEngine.Events;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    public enum Direction
    {
        Reverse = -1,
        Toggle = 0,
        Forward = 1
    }

    public abstract class Tweener : MonoBehaviour
    {
        public enum MethodType
        {
            Linear,
            EaseIn,
            EaseOut,
            EaseInOut,
            BounceIn,
            BounceOut
        }

        public enum StyleType
        {
            Once,
            Loop,
            PingPong,
            PingPongOnce
        }

        //Current tween that triggered the callback function.
        public static Tweener Current;

        [HideInInspector]
        public AnimationCurve AnimationCurve = new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 1f),
            new Keyframe(1f, 1f, 1f, 0f));

        [HideInInspector] public string CallWhenFinished;
        [HideInInspector] public float Delay;
        [HideInInspector] public float Duration = 1f;
        [HideInInspector] public string Note;

        [HideInInspector] public bool IgnoreTimeScale = false;
        private float _amountPerDelta = 1000f;
        private float _duration;

        [HideInInspector] public MethodType Method = MethodType.Linear;
        private float _factor;

        private bool _started;
        private float _startTime;

        [HideInInspector] public UnityEvent OnFinished = new UnityEvent();

        [HideInInspector] public bool SteeperCurves = false;

        [HideInInspector] public StyleType Style = StyleType.Once;

        [NonSerialized] public float TimeScale = 1f;

        [HideInInspector] public int TweenGroup = 0;

        [HideInInspector]
        [Tooltip(
            "By default, Update() will be used for tweening. Setting this to 'true' will make the tween happen in FixedUpdate() insted.")]
        public bool UseFixedUpdate = false;

        public float AmountPerDelta
        {
            get
            {
                if (Mathf.Approximately(Duration, 0f))
                    return 1000f;

                if (!Mathf.Approximately(_duration, Duration))
                {
                    _duration = Duration;
                    _amountPerDelta = Mathf.Abs(1f / Duration) * Mathf.Sign(_amountPerDelta);
                }

                return _amountPerDelta;
            }
        }

        public float TweenFactor
        {
            get => _factor;
            set => _factor = Mathf.Clamp01(value);
        }

        public Direction Direction => AmountPerDelta < 0f ? Direction.Reverse : Direction.Forward;

        private void Reset()
        {
            if (_started)
                return;

            SetStartToCurrentValue();
            SetEndToCurrentValue();
        }

        protected virtual void Start() => DoUpdate();

        protected void Update()
        {
            if (!UseFixedUpdate)
                DoUpdate();
        }

        protected void FixedUpdate()
        {
            if (UseFixedUpdate)
                DoUpdate();
        }

        protected void DoUpdate()
        {
            var delta = IgnoreTimeScale && !UseFixedUpdate ? Time.unscaledDeltaTime : Time.deltaTime;
            var time = IgnoreTimeScale && !UseFixedUpdate ? Time.unscaledTime : Time.time;

            if (!_started)
            {
                delta = 0;
                _started = true;
                _startTime = time + Delay;
            }

            if (time < _startTime)
                return;

            // Advance the sampling factor
            _factor += Mathf.Approximately(Duration, 0f) ? 1f : AmountPerDelta * delta * TimeScale;

            switch (Style)
            {
                // Loop style simply resets the play factor after it exceeds 1.
                case StyleType.Loop when _factor > 1f:
                    _factor -= Mathf.Floor(_factor);
                    break;
                // Ping-pong style reverses the direction
                case StyleType.PingPongOnce when _factor > 1f:
                case StyleType.PingPong when _factor > 1f:
                    _factor = 1f - (_factor - Mathf.Floor(_factor));
                    _amountPerDelta = -_amountPerDelta;
                    break;
                case StyleType.PingPong when _factor < 0f:
                    _factor = -_factor;
                    _factor -= Mathf.Floor(_factor);
                    _amountPerDelta = -_amountPerDelta;
                    break;
                case StyleType.PingPongOnce when _factor < 0f:
                    _factor = -_factor;
                    _factor -= Mathf.Floor(_factor);
                    _amountPerDelta = -_amountPerDelta;

                    Stop();
                    OnFinished?.Invoke();
                    break;
            }

            // If the factor goes out of range and this is a one-time tweening operation, disable the script
            if (Style == StyleType.Once
                && (Mathf.Approximately(Duration, 0f) || _factor > 1f || _factor < 0f))
            {
                _factor = Mathf.Clamp01(_factor);
                Sample(_factor, true);
                Stop();

                if (Current == this)
                    return;

                var before = Current;
                Current = this;

                // Notify the listener delegates
                OnFinished?.Invoke();

                Current = before;
            }
            else
            {
                Sample(_factor, false);
            }
        }

        public void AddOnFinished(UnityAction del) => OnFinished.AddListener(del);

        public void RemoveOnFinished(UnityAction del) => OnFinished.RemoveListener(del);

        private void OnDisable() => _started = false;

        public void Finish()
        {
            if (!enabled)
                return;

            Sample(_amountPerDelta > 0f ? 1f : 0f, true);
            Stop();
        }

        public void Stop() => enabled = false;

        public void Sample(float factor, bool isFinished)
        {
            // Calculate the sampling value
            var val = Mathf.Clamp01(factor);

            if (Method == MethodType.EaseIn)
            {
                val = 1f - Mathf.Sin(0.5f * Mathf.PI * (1f - val));
                if (SteeperCurves)
                    val *= val;
            }
            else if (Method == MethodType.EaseOut)
            {
                val = Mathf.Sin(0.5f * Mathf.PI * val);

                if (SteeperCurves)
                {
                    val = 1f - val;
                    val = 1f - val * val;
                }
            }
            else if (Method == MethodType.EaseInOut)
            {
                const float pi2 = Mathf.PI * 2f;
                val -= Mathf.Sin(val * pi2) / pi2;

                if (SteeperCurves)
                {
                    val = val * 2f - 1f;
                    var sign = Mathf.Sign(val);
                    val = 1f - Mathf.Abs(val);
                    val = 1f - val * val;
                    val = sign * val * 0.5f + 0.5f;
                }
            }
            else if (Method == MethodType.BounceIn)
            {
                val = BounceLogic(val);
            }
            else if (Method == MethodType.BounceOut)
            {
                val = 1f - BounceLogic(1f - val);
            }

            OnUpdate(AnimationCurve?.Evaluate(val) ?? val, isFinished);
        }

        private static float BounceLogic(float val)
        {
            if (val < 0.363636f) // 0.363636 = (1/ 2.75)
                val = 7.5685f * val * val;
            else if (val < 0.727272f) // 0.727272 = (2 / 2.75)
                val = 7.5625f * (val -= 0.545454f) * val + 0.75f; // 0.545454f = (1.5 / 2.75) 
            else if (val < 0.909090f) // 0.909090 = (2.5 / 2.75) 
                val = 7.5625f * (val -= 0.818181f) * val + 0.9375f; // 0.818181 = (2.25 / 2.75) 
            else
                val = 7.5625f * (val -= 0.9545454f) * val + 0.984375f; // 0.9545454 = (2.625 / 2.75) 
            return val;
        }

        [ContextMenu("Play forward")] public void PlayForward() => Play(true);

        [ContextMenu("Play reverse")] private void PlayReverseContextMenuCommand() => PlayReverse();

        public void PlayReverse(bool ignoreDelay = false)
        {
            if (this == null)
                return;

            var delay = Delay;
            if (ignoreDelay)
                Delay = 0;

            Play(false);
            Delay = delay;
        }

        public virtual void Play(bool forward)
        {
            if (this == null)
                return;

            _amountPerDelta = Mathf.Abs(AmountPerDelta);
            if (!forward)
                _amountPerDelta = -_amountPerDelta;

            if (!enabled)
            {
                enabled = true;
                _started = false;
            }

            DoUpdate();
        }

        [ContextMenu("Reset To Beginning")]
        public void ResetToBeginning()
        {
            _started = false;
            _factor = AmountPerDelta < 0f ? 1f : 0f;
            Sample(_factor, false);
        }

        [ContextMenu("Reset To Ending")]
        public void ResetToEnding()
        {
            _started = false;
            _factor = AmountPerDelta > 0f ? 1f : 0f;
            Sample(_factor, false);
        }

        //Manually start the tweening process, reversing its direction.
        public void Toggle()
        {
            if (_factor > 0f)
                _amountPerDelta = -AmountPerDelta;
            else
                _amountPerDelta = Mathf.Abs(AmountPerDelta);
            enabled = true;
        }

        protected abstract void OnUpdate(float factor, bool isFinished);

        public static T Begin<T>(GameObject go, float duration, float delay = 0f) where T : Tweener
        {
            var comp = go.GetComponent<T>();
            if (comp != null && comp.TweenGroup != 0)
            {
                comp = null;
                var comps = go.GetComponents<T>();
                for (int i = 0, imax = comps.Length; i < imax; ++i)
                {
                    comp = comps[i];
                    if (comp != null && comp.TweenGroup == 0)
                        break;

                    comp = null;
                }
            }

            if (comp == null)
            {
                comp = go.AddComponent<T>();

                if (comp == null)
                {
                    Debug.LogError("Unable to add " + typeof(T) + " to " + GetHierarchy(go),
                        go);
                    return null;
                }
            }

            comp._started = false;
            comp._factor = 0f;
            comp.Duration = duration;
            comp._duration = duration;
            comp.Delay = delay;
            comp._amountPerDelta = duration > 0f ? Mathf.Abs(1f / duration) : 1000f;
            comp.Style = StyleType.Once;
            comp.AnimationCurve =
                new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
            comp.CallWhenFinished = null;
            comp.OnFinished.RemoveAllListeners();
            comp.enabled = true;
            return comp;
        }

        public virtual void SetStartToCurrentValue()
        {
        }

        public virtual void SetEndToCurrentValue()
        {
        }

        public void ResetAmountPerDeltaDirection() => _amountPerDelta = Mathf.Abs(_amountPerDelta);

        [ContextMenu("Reset To Beginning Absolute")]
        public void ResetToBeginningAbsolute()
        {
            ResetAmountPerDeltaDirection();
            ResetToBeginning();
        }

        [ContextMenu("Reset To Beginning Absolute")]
        public void ResetToEndingAbsolute()
        {
            ResetAmountPerDeltaDirection();
            ResetToEnding();
        }

        [ContextMenu("Reset and play")]
        public void ResetAndPlay()
        {
            ResetToBeginning();
            PlayForward();
        }

        public void ResetAbsoluteAndPlay()
        {
            ResetToBeginningAbsolute();
            PlayForward();
        }

        public WaitForSeconds GetWait() => new(Delay + Duration);

        public WaitForSecondsRealtime GetWaitRealtime() => new(Delay + Duration);
        
        //Returns the hierarchy of the object in a human-readable format.
        private static string GetHierarchy(GameObject obj)
        {
            if (obj == null)
                return "";
            var path = obj.name;

            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = obj.name + "\\" + path;
            }

            return path;
        }
    }
}