using UnityEngine;
using UnityEngine.UI;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    [AddComponentMenu("ChaserLib/Tween/TweenFill")]
    public class TweenFill : Tweener
    {
        [Range(0f, 1f)] public float From = 1f;
        [Range(0f, 1f)] public float To = 1f;

        private Image _image;
        private bool _cached;

        public float Value
        {
            get
            {
                if (!_cached) Cache();
                return _image != null ? _image.fillAmount : 0f;
            }
            set
            {
                if (!_cached) Cache();
                if (_image != null) _image.fillAmount = value;
            }
        }

        private void Cache()
        {
            _cached = true;
            _image = GetComponent<Image>();
        }

        protected override void OnUpdate(float factor, bool isFinished) => Value = Mathf.Lerp(From, To, factor);

        public static TweenFill Begin(GameObject go, float duration, float fill)
        {
            var comp = Begin<TweenFill>(go, duration);
            comp.From = comp.Value;
            comp.To = fill;

            if (duration > 0f)
                return comp;

            comp.Sample(1f, true);
            comp.enabled = false;

            return comp;
        }

        public override void SetStartToCurrentValue() => From = Value;
        public override void SetEndToCurrentValue() => To = Value;
    }
}