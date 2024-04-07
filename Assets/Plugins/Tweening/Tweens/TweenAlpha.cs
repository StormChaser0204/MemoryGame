using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    [AddComponentMenu("ChaserLib/Tween/TweenAlpha")]
    public class TweenAlpha : Tweener
    {
        [Range(0f, 1f)] public float From = 1f;
        [Range(0f, 1f)] public float To = 1f;

        private CanvasGroup _canvasGroup;
        private Graphic _graphic;
        private TMP_Text _textMesh;
        private SpriteRenderer _spr;
        private Material _mat;

        private bool _cached;

        public float Value
        {
            get
            {
                if (!_cached)
                    Cache();

                if (_canvasGroup != null)
                    return _canvasGroup.alpha;
                if (_graphic != null)
                    return _graphic.color.a;
                if (_textMesh != null)
                    return _textMesh.alpha;
                if (_spr != null)
                    return _spr.color.a;

                return _mat != null ? _mat.color.a : 1f;
            }
            set
            {
                if (!_cached)
                    Cache();

                if (_canvasGroup != null)
                    _canvasGroup.alpha = value;
                if (_graphic != null)
                {
                    var c = _graphic.color;
                    c.a = value;
                    _graphic.color = c;
                }
                else if (_textMesh != null)
                {
                    var c = _textMesh.color;
                    c.a = value;
                    _textMesh.color = c;
                }
                else if (_spr != null)
                {
                    var c = _spr.color;
                    c.a = value;
                    _spr.color = c;
                }
                else if (_mat != null)
                {
                    var c = _mat.color;
                    c.a = value;
                    _mat.color = c;
                }
            }
        }

        private void Cache()
        {
            _cached = true;

            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup != null)
                return;

            _graphic = GetComponent<Graphic>();
            if (_graphic != null)
                return;

            _textMesh = GetComponent<TMP_Text>();
            if (_textMesh != null)
                return;

            _spr = GetComponent<SpriteRenderer>();
            if (_spr != null)
                return;

            var ren = GetComponent<Renderer>();
            if (ren != null)
            {
                _mat = ren.material;
                return;
            }

            if (_mat == null)
                _canvasGroup = GetComponentInChildren<CanvasGroup>();
        }

        protected override void OnUpdate(float factor, bool isFinished) => Value = Mathf.Lerp(From, To, factor);

        public static TweenAlpha Begin(GameObject go, float duration, float alpha, float delay = 0f)
        {
            var comp = Begin<TweenAlpha>(go, duration, delay);
            comp.From = comp.Value;
            comp.To = alpha;

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
    }
}