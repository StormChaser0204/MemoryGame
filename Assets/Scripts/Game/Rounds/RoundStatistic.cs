namespace Game.Rounds
{
    internal class RoundStatistic
    {
        public int TotalPairsAmount;
        public int CurrentPairsAmount;

        public int CorrectAmount { get; private set; }
        public int IncorrectAmount { get; private set; }
        public int TotalTriesAmount { get; private set; }

        public void IncCorrect() => CorrectAmount++;
        public void IncIncorrect() => IncorrectAmount++;
        public void IncTriesCount() => TotalTriesAmount++;

        public void Reset()
        {
            CorrectAmount = 0;
            IncorrectAmount = 0;
            TotalTriesAmount = 0;
            CurrentPairsAmount = 0;
        }
    }
}