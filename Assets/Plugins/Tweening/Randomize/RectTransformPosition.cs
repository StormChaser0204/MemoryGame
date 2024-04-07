using Dependencies.ChaserLib.Tweening.Tweens;
using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Randomize
{
    [RequireComponent(typeof(IRandomizer))]
    [RequireComponent(typeof(TweenAnchoredPosition))]
    [AddComponentMenu("ChaserLib/Tweening/Randomize Tween Rect Transform Position")]
    public class RectTransformPosition : PositionRandomizer
    {
        private TweenAnchoredPosition _tween;

        protected override void Awake()
        {
            _tween = GetComponent<TweenAnchoredPosition>();
            base.Awake();
        }

        protected override Vector3 GetCurrentPosition() => _tween.Value;

        public override void Randomize()
        {
            _tween.SetStartToCurrentValue();
            _tween.To = GetNewTo(_tween.From);

            base.Randomize();
        }

        protected override Tweener GetTween() => _tween;
    }
}