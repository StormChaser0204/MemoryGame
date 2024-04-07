using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Randomize
{
    public abstract class RandomizeBase : MonoBehaviour
    {
        [SerializeField] private bool _autoStart = true;

        protected virtual void Awake()
        {
            if (_autoStart)
                Randomize();
        }

        public abstract void Randomize();
    }
}