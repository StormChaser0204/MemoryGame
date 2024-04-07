using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    [AddComponentMenu("ChaserLib/Tween/TweenRotation")]
    public class TweenRotation : Tweener
    {
        public Vector3 From;
        public Vector3 To;
        public bool QuaternionLerp = false;

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

        public Quaternion Value
        {
            get => CachedTransform.localRotation;
            set => CachedTransform.localRotation = value;
        }

        protected override void OnUpdate(float factor, bool isFinished) =>
            Value = QuaternionLerp
                ? Quaternion.SlerpUnclamped(Quaternion.Euler(From), Quaternion.Euler(To), factor)
                : Quaternion.Euler(new Vector3(
                    Mathf.LerpUnclamped(From.x, To.x, factor),
                    Mathf.LerpUnclamped(From.y, To.y, factor),
                    Mathf.LerpUnclamped(From.z, To.z, factor)));

        public static TweenRotation Begin(GameObject go, float duration, Quaternion rot)
        {
            var comp = Begin<TweenRotation>(go, duration);
            comp.From = comp.Value.eulerAngles;
            comp.To = rot.eulerAngles;

            if (duration > 0f)
                return comp;

            comp.Sample(1f, true);
            comp.enabled = false;

            return comp;
        }

        [ContextMenu("Set 'From' to current value")]
        public override void SetStartToCurrentValue() => From = Value.eulerAngles;

        [ContextMenu("Set 'To' to current value")]
        public override void SetEndToCurrentValue() => To = Value.eulerAngles;

        [ContextMenu("Assume value of 'From'")]
        private void SetCurrentValueToStart() => Value = Quaternion.Euler(From);

        [ContextMenu("Assume value of 'To'")] private void SetCurrentValueToEnd() => Value = Quaternion.Euler(To);

        public void ResetToAndPlay(Vector3 newTo) => Reset(Value.eulerAngles, newTo);

        public void Reset(Vector3 newFrom, Vector3 newTo)
        {
            From = newFrom;
            To = newTo;
            ResetAndPlay();
        }
    }
}