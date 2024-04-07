using System;
using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class RectTransformVector2Tween : Tweener
    {
        public Vector2 From;
        public Vector2 To;
        public Component VectorComponent;
        
        public enum Component
        {
            Both = 0,
            X = 1,
            Y = 2,
        }
        
        public RectTransform CachedTransform
        {
            get
            {
                if (_transf == null)
                    _transf = GetComponent<RectTransform>();

                return _transf;
            }
        }

        private RectTransform _transf;

        public abstract Vector2 Value { get; set; }

        protected override void OnUpdate(float factor, bool isFinished)
        {
            var newValue = From * (1f - factor) + To * factor;
            switch (VectorComponent)
            {
                case Component.Both:
                    break;
                case Component.X:
                    newValue.y = Value.y;
                    break;
                case Component.Y:
                    newValue.x = Value.x;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Value = newValue;
        }

        protected static T Begin<T>(GameObject go, float duration, Vector3 pos)
            where T : RectTransformVector2Tween
        {
            var comp = Begin<T>(go, duration);
            comp.From = comp.Value;
            comp.To = pos;

            if (duration > 0f)
                return comp;

            comp.Sample(1f, true);
            comp.enabled = false;

            return comp;
        }

        [ContextMenu("Set 'From' to current value")]
        public override void SetStartToCurrentValue() => From = Value;

        [ContextMenu("Set 'To' to current value")]
        public override void SetEndToCurrentValue() => To = Value;

        [ContextMenu("Assume value of 'From'")]
        private void SetCurrentValueToStart() => Value = From;

        [ContextMenu("Assume value of 'To'")] private void SetCurrentValueToEnd() => Value = To;

        public void ResetToAndPlay(Vector2 newTo) => ResetAndPlay(Value, newTo);

        public void ResetAndPlay(Vector2 newFrom, Vector2 newTo)
        {
            From = newFrom;
            To = newTo;
            ResetAndPlay();
        }
    }
}