using UnityEngine;

namespace Dependencies.ChaserLib.Tweening
{
    public class RestartTweensOnEnable : MonoBehaviour
    {
        [SerializeField] private TweenBatcher _batcher;

        private void OnEnable()
        {
            _batcher.ResetToBeginning();
            _batcher.PlayForward();
        }
    }
}