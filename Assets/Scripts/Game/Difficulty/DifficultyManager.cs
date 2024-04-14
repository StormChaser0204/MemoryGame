namespace Game.Difficulty
{
    internal class DifficultyManager
    {
        public float Difficulty { get; private set; }

        public DifficultyManager(float current)
        {
            Difficulty = current;
        }

        public void ChangeDifficulty(float additionalValue)
        {
            Difficulty += additionalValue;
        }
    }
}