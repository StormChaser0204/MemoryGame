using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Randomize
{
    [AddComponentMenu("ChaserLib/Tweening/Vector3 Randomizer")]
    public class Vector3Randomizer : MonoBehaviour, IRandomizer
    {
        [SerializeField] private Vector3 _range;

        public Vector3 Roll()
        {
            var result = _range;
            for (var i = 0; i < 3; i++)
                result[i] *= Random.Range(-1f, 1f);

            return result;
        }
    }
}