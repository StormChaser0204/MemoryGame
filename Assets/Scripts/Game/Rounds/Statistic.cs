namespace Game.Rounds
{
    internal class Statistic
    {
        private int _correctAmount;
        private int _incorrectAmount;

        public void IncCorrect() => _correctAmount++;
        public void IncIncorrect() => _incorrectAmount++;
    }
}