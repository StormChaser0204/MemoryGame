using UnityEngine;
using UnityEngine.UI;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    [RequireComponent(typeof(Slider))]
    public class TweenSlider : Tweener
    {
        [Range(0f, 1f)] public float From = 1f;
        [Range(0f, 1f)] public float To = 1f;

        private Slider _slider;

        public Slider Slider
        {
            get
            {
                if (_slider != null)
                    return _slider;

                _slider = GetComponent<Slider>();

                if (_slider != null)
                    return _slider;

                Debug.LogError("TweenSlider needs a Slider to work with", this);
                enabled = false;

                return null;
            }
        }

        public float Value
        {
            get => Slider != null ? Slider.value : 0f;
            set
            {
                if (Slider == null)
                    return;

                Slider.value = Mathf.Clamp01(value);
            }
        }

        protected override void OnUpdate(float factor, bool isFinished) => Value = From * (1f - factor) + To * factor;

        public static TweenSlider Begin(GameObject go, float duration, float targetValue)
        {
            var comp = Begin<TweenSlider>(go, duration);
            comp.From = comp.Value;
            comp.To = targetValue;
            return comp;
        }

        public override void SetStartToCurrentValue() => From = Value;
        
        public override void SetEndToCurrentValue() => To = Value;

        public void ResetToAndPlay(float newTo) => Reset(Value, newTo);

        public void Reset(float newFrom, float newTo)
        {
            From = newFrom;
            To = newTo;
            ResetAndPlay();
        }
    }
}