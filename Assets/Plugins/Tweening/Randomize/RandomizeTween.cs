using Dependencies.ChaserLib.Tweening.Tweens;
using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Randomize
{
    public abstract class RandomizeTween : RandomizeBase
    {
        [SerializeField] private bool _isLooped;

        protected IRandomizer Randomizer;
        
        protected abstract Tweener GetTween();

        protected override void Awake()
        {
            Randomizer = GetComponent<IRandomizer>();
            
            if (_isLooped)
                GetTween().AddOnFinished(Randomize);
            
            base.Awake();
        }

        public override void Randomize()
        {
            var t = GetTween();
            t.ResetToBeginning();
            t.PlayForward();
        }
    }
}