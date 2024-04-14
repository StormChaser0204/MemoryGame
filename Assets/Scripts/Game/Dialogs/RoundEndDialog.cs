using System;
using Dependencies.ChaserLib.Dialogs;
using Dependencies.ChaserLib.ServiceLocator;
using Game.Rounds.Events;
using Game.Score;
using Game.Timer;
using Plugins.EventDispatching.Dispatcher;
using TMPro;
using UnityEngine;

namespace Game.Dialogs
{
    internal class RoundEndDialog : DialogBase
    {
        [SerializeField] private TMP_Text _time;
        [SerializeField] private TMP_Text _score;
        [SerializeField] private TMP_Text _coins;

        private static ServiceLocator Locator => ServiceLocator.Instance;
        private static IEventDispatcher Dispatcher => Locator.Get<IEventDispatcher>();
        private static TimerManager TimerManager => Locator.Get<TimerManager>();
        private static ScoreCounter ScoreCounter => Locator.Get<ScoreCounter>();

        public override void Show()
        {
            var timeSpan = TimeSpan.FromSeconds(TimerManager.GetCurrentTime());
            _time.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            _score.text = ScoreCounter.Get().ToString();
            base.Show();
        }

        public void LoadNextRound()
        {
            Dispatcher.Raise(new StartRoundEvent());
            Hide();
        }
    }
}