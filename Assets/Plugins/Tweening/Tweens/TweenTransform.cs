using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    [AddComponentMenu("ChaserLib/Tween/TweenTransform")]
    public class TweenTransform : Tweener
    {
        public Transform From;
        public Transform To;

        public bool ParentWhenFinished = false;

        private Vector3 _pos;
        private Quaternion _rot;
        private Vector3 _scale;
        private Transform _trans;

        protected override void OnUpdate(float factor, bool isFinished)
        {
            if (To == null)
                return;

            if (_trans == null)
            {
                _trans = transform;
                _pos = _trans.position;
                _rot = _trans.rotation;
                _scale = _trans.localScale;
            }

            if (From != null)
            {
                _trans.position = From.position * (1f - factor) + To.position * factor;
                _trans.localScale = From.localScale * (1f - factor) + To.localScale * factor;
                _trans.rotation = Quaternion.Slerp(From.rotation, To.rotation, factor);
            }
            else
            {
                _trans.position = _pos * (1f - factor) + To.position * factor;
                _trans.localScale = _scale * (1f - factor) + To.localScale * factor;
                _trans.rotation = Quaternion.Slerp(_rot, To.rotation, factor);
            }

            if (ParentWhenFinished && isFinished)
                _trans.parent = To;
        }

        public static TweenTransform Begin(GameObject go, float duration, Transform to) =>
            Begin(go, duration, null, to);

        public static TweenTransform Begin(GameObject go, float duration, Transform from, Transform to)
        {
            var comp = Begin<TweenTransform>(go, duration);
            comp.From = from;
            comp.To = to;

            if (duration > 0f)
                return comp;

            comp.Sample(1f, true);
            comp.enabled = false;

            return comp;
        }
    }
}