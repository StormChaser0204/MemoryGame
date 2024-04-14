using System;

namespace Game.Score
{
    internal class ScoreCounter
    {
        //change to ReactiveProperty
        private int _currentScore;

        public void Add(int value)
        {
            _currentScore = Math.Clamp(_currentScore + value, 0, int.MaxValue);
        }

        public int Get() => _currentScore;
    }
}