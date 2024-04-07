using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    [AddComponentMenu("ChaserLib/Tween/TweenPositionComponent")]
    public class TweenPositionComponent : Tweener
    {
        public enum Component
        {
            X = 0,
            Y = 1,
            Z = 2
        }

        public float From;
        public float To;
        public Component TargetComponent;
        public bool WorldSpace;

        public Transform CachedTransform
        {
            get
            {
                if (_trans == null) _trans = transform;
                return _trans;
            }
        }
        private Transform _trans;

        public float Value
        {
            get
            {
                var pos = WorldSpace ? CachedTransform.position : CachedTransform.localPosition;
                return pos[(int) TargetComponent];
            }
            set
            {
                if (WorldSpace)
                    SetWorldPosition(CachedTransform, value);
                else
                    SetLocalPosition(CachedTransform, value);
            }
        }

        private void SetWorldPosition(Transform t, float v) => t.position = SetAxis(v, t.position);

        private void SetLocalPosition(Transform t, float v) => t.localPosition = SetAxis(v, t.localPosition);

        private Vector3 SetAxis(float v, Vector3 pos)
        {
            switch (TargetComponent)
            {
                case Component.X:
                    pos.x = v;
                    break;
                case Component.Y:
                    pos.y = v;
                    break;
                case Component.Z:
                    pos.z = v;
                    break;
            }

            return pos;
        }

        protected override void OnUpdate(float factor, bool isFinished) => Value = From * (1f - factor) + To * factor;

        public static TweenPositionComponent Begin(GameObject go, float duration, float v, Component comp)
        {
            var tween = Begin<TweenPositionComponent>(go, duration);
            tween.TargetComponent = comp;
            tween.From = tween.Value;
            tween.To = v;

            if (duration > 0f)
                return tween;

            tween.Sample(1f, true);
            tween.enabled = false;

            return tween;
        }

        public static TweenPositionComponent Begin(GameObject go, float duration, float v, Component comp,
            bool worldSpace)
        {
            var tween = Begin<TweenPositionComponent>(go, duration);
            tween.TargetComponent = comp;
            tween.WorldSpace = worldSpace;
            tween.From = tween.Value;
            tween.To = v;

            if (duration > 0f)
                return tween;

            tween.Sample(1f, true);
            tween.enabled = false;

            return tween;
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