using UnityEngine;
using UnityEngine.UI;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    [AddComponentMenu("ChaserLib/Tween/Tween Color")]
    public class TweenColor : Tweener
    {
        public Color From = Color.white;
        public Color To = Color.white;
        public bool UseCache = true;

        private bool CacheRequired => !UseCache || !_cached;

        private Graphic _graphic;
        private SpriteRenderer _sprite;
        private Material _mat;
        private Light _light;

        private bool _cached;

        public Color Value
        {
            get
            {
                if (CacheRequired)
                    Cache();
                if (_graphic != null)
                    return _graphic.color;
                if (_mat != null)
                    return _mat.color;
                if (_sprite != null)
                    return _sprite.color;
                if (_light != null)
                    return _light.color;

                return Color.black;
            }
            set
            {
                if (CacheRequired)
                    Cache();
                if (_graphic != null)
                {
                    _graphic.color = value;
                }
                else if (_mat != null)
                {
                    _mat.color = value;
                }
                else if (_sprite != null)
                {
                    _sprite.color = value;
                }
                else if (_light != null)
                {
                    _light.color = value;
                    _light.enabled = value.r + value.g + value.b > 0.01f;
                }
            }
        }

        private void Cache()
        {
            _cached = true;
            _graphic = GetComponent<Graphic>();
            if (_graphic != null)
                return;

            _sprite = GetComponent<SpriteRenderer>();
            if (_sprite != null)
                return;

            var ren = GetComponent<Renderer>();
            if (ren != null)
            {
                _mat = ren.material;
                return;
            }

            _light = GetComponent<Light>();
            if (_light == null)
                _graphic = GetComponentInChildren<Graphic>();
        }

        protected override void OnUpdate(float factor, bool isFinished) =>
            Value = Color.Lerp(From, To, factor);

        public static TweenColor Begin(GameObject go, float duration, Color color)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return null;
#endif
            var comp = Begin<TweenColor>(go, duration);
            comp.From = comp.Value;
            comp.To = color;

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

        public void ResetToAndPlay(Color newTo) => Reset(Value, newTo);

        public void Reset(Color newFrom, Color newTo)
        {
            From = newFrom;
            To = newTo;
            ResetAndPlay();
        }
    }
}