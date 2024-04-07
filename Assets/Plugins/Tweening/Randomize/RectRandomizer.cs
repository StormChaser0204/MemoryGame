using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Randomize
{
    [AddComponentMenu("ChaserLib/Tweening/Rect Position Randomizer")]
    public class RectRandomizer : MonoBehaviour, IRandomizer
    {
        [SerializeField] private Rect _rect;

        public Vector3 Roll()
        {
            var x = Random.Range(_rect.xMin, _rect.xMax);
            var y = Random.Range(_rect.yMin, _rect.yMax);
            return new Vector2(x, y);
        }
    }
}