using Dependencies.ChaserLib.Dialogs;
using Dependencies.ChaserLib.ServiceLocator;
using Game.Difficulty;
using JetBrains.Annotations;
using Plugins.EventHandler;
using Plugins.EventHandler.Handlers;
using UnityEngine;

namespace Game.Rounds.Handlers
{
    [UsedImplicitly]
    internal class FinishRoundHandler : EventHandler
    {
        private static ServiceLocator Locator => ServiceLocator.Instance;
        private static IDialogsLauncher DialogsLauncher => Locator.Get<IDialogsLauncher>();
        private static RoundStatistic RoundStatistic => Locator.Get<RoundStatistic>();
        private static DifficultyManager DifficultyManager => Locator.Get<DifficultyManager>();

        public FinishRoundHandler(IEvent ev) : base(ev)
        {
        }

        public override void Handle()
        {
            var roundAccuracy =
                (float) RoundStatistic.TotalPairsAmount / RoundStatistic.TotalTriesAmount;
            var additionalDifficulty = CountAdditionalDifficulty(roundAccuracy);
            DifficultyManager.ChangeDifficulty(additionalDifficulty);
            Debug.Log("Accuracy: " + roundAccuracy);
            Debug.Log("Additional difficulty: " + additionalDifficulty);
            Debug.Log("New difficulty: " + DifficultyManager.Difficulty);

            DialogsLauncher.Show(DialogType.FinnishRound);
        }

        private static float CountAdditionalDifficulty(float accuracy)
        {
            if (accuracy >= 0.6)
                return 1f;

            if (accuracy <= 0.4f)
                return -1f;

            return 0.5f;
        }
    }
}