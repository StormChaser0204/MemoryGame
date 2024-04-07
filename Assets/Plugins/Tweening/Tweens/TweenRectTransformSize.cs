using UnityEngine;
using UnityEngine.UI;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    [AddComponentMenu("ChaserLib/Tween/Tween RectTransform Size")]
    public class TweenRectTransformSize : RectTransformVector2Tween
    {
        public override Vector2 Value
        {
            get => CachedTransform.sizeDelta;
            set
            {
                CachedTransform.sizeDelta = value;
                LayoutRebuilder.MarkLayoutForRebuild(CachedTransform);
            }
        }

        public static TweenRectTransformSize Begin(GameObject go, float duration, Vector3 pos) =>
            Begin<TweenRectTransformSize>(go, duration, pos);
    }
}