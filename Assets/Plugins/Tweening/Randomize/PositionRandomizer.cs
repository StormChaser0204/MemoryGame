using System;
using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Randomize
{
    public abstract class PositionRandomizer : RandomizeTween
    {
        private enum Mode
        {
            Additive,
            Absolute,
            AdditiveToInitial
        }

        [SerializeField] private Mode _mode;

        private Vector3 _initialPosition;

        protected override void Awake()
        {
            _initialPosition = GetCurrentPosition();

            base.Awake();
        }

        protected abstract Vector3 GetCurrentPosition();

        protected Vector3 GetNewTo(Vector3 currentPos) =>
            _mode switch
            {
                Mode.Additive => currentPos + Randomizer.Roll(),
                Mode.Absolute => Randomizer.Roll(),
                Mode.AdditiveToInitial => _initialPosition + Randomizer.Roll(),
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}