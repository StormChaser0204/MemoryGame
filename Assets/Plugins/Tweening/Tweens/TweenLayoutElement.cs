using UnityEngine;
using UnityEngine.UI;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    [AddComponentMenu("ChaserLib/Tween/Tween Layout Element")]
    [RequireComponent(typeof(LayoutElement))]
    public class TweenLayoutElement : Tweener
    {
        public Field FieldForTweening;
        public float From;
        public float To;

        public LayoutElement CachedLayoutElement
        {
            get
            {
                if (_layoutElement == null)
                    _layoutElement = GetComponent<LayoutElement>();
                return _layoutElement;
            }
        }

        private LayoutElement _layoutElement;

        public float Value
        {
            get => GetField(FieldForTweening);
            set => UpdateField(FieldForTweening, value);
        }

        protected override void OnUpdate(float factor, bool isFinished) =>
            Value = From * (1f - factor) + To * factor;

        private float GetField(Field field)
        {
            var targetValue = field switch
            {
                Field.MinWidth => CachedLayoutElement.minWidth,
                Field.MinHeight => CachedLayoutElement.minHeight,
                Field.PreferredWidth => CachedLayoutElement.preferredWidth,
                Field.PreferredHeight => CachedLayoutElement.preferredHeight,
                Field.FlexibleWidth => CachedLayoutElement.flexibleWidth,
                Field.FlexibleHeight => CachedLayoutElement.flexibleHeight,
                _ => 0
            };

            return targetValue;
        }

        private void UpdateField(Field field, float value)
        {
            switch (field)
            {
                case Field.MinWidth:
                    CachedLayoutElement.minWidth = value;
                    break;
                case Field.MinHeight:
                    CachedLayoutElement.minHeight = value;
                    break;
                case Field.PreferredWidth:
                    CachedLayoutElement.preferredWidth = value;
                    break;
                case Field.PreferredHeight:
                    CachedLayoutElement.preferredHeight = value;
                    break;
                case Field.FlexibleWidth:
                    CachedLayoutElement.flexibleWidth = value;
                    break;
                case Field.FlexibleHeight:
                    CachedLayoutElement.flexibleHeight = value;
                    break;
            }
        }

        public enum Field
        {
            MinWidth = 0,
            MinHeight = 1,
            PreferredWidth = 2,
            PreferredHeight = 3,
            FlexibleWidth = 4,
            FlexibleHeight = 5,
        }
    }
}