using TMPro;
using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    [AddComponentMenu("ChaserLib/Tween/TweenText")]
    public class TweenText : Tweener
    {
        public string From;
        public string To;

        private TextMeshProUGUI CachedText
        {
            get
            {
                if (_text == null)
                    _text = GetComponent<TextMeshProUGUI>();
                return _text;
            }
        }

        private TextMeshProUGUI _text;

        public string Value
        {
            get => CachedText.text;
            set => CachedText.text = value;
        }

        protected override void OnUpdate(float factor, bool isFinished) =>
            Value = GetStringByFactor(factor);

        private string GetStringByFactor(float factor) =>
            To.Substring(0, (int) (To.Length * factor));

        public static TweenText Begin(GameObject go, float duration, string text)
        {
            var comp = Begin<TweenText>(go, duration);
            comp.From = comp.Value;
            comp.To = text;

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