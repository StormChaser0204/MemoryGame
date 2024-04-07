using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    [AddComponentMenu("ChaserLib/Tween/Tween AnchoredPosition")]
    [RequireComponent(typeof(RectTransform))]
    public class TweenAnchoredPosition : RectTransformVector2Tween
    {
        public override Vector2 Value
        {
            get => CachedTransform.anchoredPosition;
            set => CachedTransform.anchoredPosition = value;
        }
        
        public static TweenAnchoredPosition Begin(GameObject go, float duration, Vector3 pos) =>
            Begin<TweenAnchoredPosition>(go, duration, pos);
    }
}