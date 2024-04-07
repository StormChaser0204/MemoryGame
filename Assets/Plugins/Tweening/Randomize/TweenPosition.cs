using Dependencies.ChaserLib.Tweening.Tweens;
using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Randomize
{
    [RequireComponent(typeof(Tweens.TweenPosition))]
    [AddComponentMenu("ChaserLib/Tweening/Randomize Tween Position")]
    public class TweenPosition : PositionRandomizer
    {
        private Tweens.TweenPosition _tween;

        protected override void Awake()
        {
            _tween = GetComponent<Tweens.TweenPosition>();
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