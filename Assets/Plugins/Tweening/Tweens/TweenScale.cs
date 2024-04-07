using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    [AddComponentMenu("ChaserLib/Tween/Tween Scale")]
    public class TweenScale : Tweener
    {
        public Vector3 From = Vector3.one;
        public Vector3 To = Vector3.one;

        public Transform CachedTransform
        {
            get
            {
                if (_mTrans == null)
                    _mTrans = transform;

                return _mTrans;
            }
        }

        private Transform _mTrans;

        public Vector3 Value
        {
            get => CachedTransform.localScale;
            set => CachedTransform.localScale = value;
        }

        protected override void OnUpdate(float factor, bool isFinished) => Value = From * (1f - factor) + To * factor;

        public static TweenScale Begin(GameObject go, float duration, Vector3 scale)
        {
            var comp = Begin<TweenScale>(go, duration);
            comp.From = comp.Value;
            comp.To = scale;

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
    }
}