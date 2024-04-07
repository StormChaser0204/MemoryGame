using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    [AddComponentMenu("ChaserLib/Tween/TweenPosition")]
    public class TweenPosition : Tweener
    {
        public Vector3 From;
        public Vector3 To;
        public bool WorldSpace;

        private Transform _trans;

        public Transform CachedTransform
        {
            get
            {
                if (_trans == null) _trans = transform;
                return _trans;
            }
        }

        public Vector3 Value
        {
            get => WorldSpace ? CachedTransform.position : CachedTransform.localPosition;
            set
            {
                if (WorldSpace) CachedTransform.position = value;
                else CachedTransform.localPosition = value;
            }
        }

        protected override void OnUpdate(float factor, bool isFinished) => Value = From * (1f - factor) + To * factor;

        public static TweenPosition Begin(GameObject go, float duration, Vector3 to, bool worldSpace = false)
        {
            var comp = Begin<TweenPosition>(go, duration);
            comp.WorldSpace = worldSpace;
            comp.From = comp.Value;
            comp.To = to;

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

        [ContextMenu("Assume value of 'To'")]
        private void SetCurrentValueToEnd() => Value = To;

        public void ResetToAndPlay(Vector3 newTo) => ResetAndPlay(Value, newTo);

        public void ResetAndPlay(Vector3 newFrom, Vector3 newTo)
        {
            From = newFrom;
            To = newTo;
            ResetAndPlay();
        }
    }
}