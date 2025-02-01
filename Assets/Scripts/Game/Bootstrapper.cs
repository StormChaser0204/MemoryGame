using System;
using Common.Application;
using Common.Cards;
using Cysharp.Threading.Tasks;
using Dependencies.ChaserLib.Dialogs;
using Dependencies.ChaserLib.ServiceLocator;
using Dependencies.ChaserLib.Tasks;
using Game.Cards;
using Game.Cards.Events;
using Game.Difficulty;
using Game.Rounds;
using Game.Rounds.Events;
using Game.Rounds.Handlers;
using Game.Score;
using Game.Timer;
using Plugins.DataHandler.Commands;
using Plugins.EventDispatching.Dispatcher;
using UnityEngine;

namespace Game
{
    [AddComponentMenu("Game/Game Bootstrapper")]
    internal class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private CardHolder _cardHolder;
        [SerializeField] private DialogsLauncher _dialogsLauncher;
        [SerializeField] private CardsData _cardsData;

        [SerializeField] private TimerView _timerView;
        [SerializeField] private ScoreView _scoreView;

        private static ServiceLocator Locator => ServiceLocator.Instance;
        private IEventDispatcher _dispatcher;

        public async void Start()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f));

            _dispatcher = new EventDispatcher();
            Locator.Add(_dispatcher);
            Locator.Add<IDialogsLauncher>(_dialogsLauncher);
            Locator.Add(_cardHolder);
            Locator.Add(_cardsData);

            var token = gameObject.GetCancellationTokenOnDestroy();
            Locator.Add<ICancellationTokenFactory>(new CancellationTokenFactory(token));

            SetupStatistic();
            SetupScoreCounter();
            SetupTimer();
            SetupDifficulty();
            SetupUnlockCards();
            
            BindHandlers();
            _dispatcher.Raise(new StartRoundEvent());
        }

        private static void SetupStatistic()
        {
            var statistic = new RoundStatistic();
            Locator.Add(statistic);
        }

        private void SetupScoreCounter()
        {
            var scoreCounter = new ScoreCounter();
            Locator.Add(scoreCounter);
            Locator.Add(_scoreView);
        }

        private void SetupTimer()
        {
            var manager = gameObject.AddComponent<TimerManager>();
            manager.BindView(_timerView);
            Locator.Add(manager);
        }

        private static void SetupDifficulty()
        {
            var manager = new DifficultyManager(0);
            Locator.Add(manager);
        }
        
        private static void SetupUnlockCards()
        {
            var unlockInfo = new LoadDataCommand<UnlockCardsData>().Execute() ?? new UnlockCardsData();
            Locator.Add(unlockInfo);
        }

        private void BindHandlers()
        {
            _dispatcher.Bind().Handler<StatisticHandler>().To<CorrectPairSelectedEvent>();
            _dispatcher.Bind().Handler<StatisticHandler>().To<IncorrectPairSelectedEvent>();

            _dispatcher.Bind().Handler<ScoreHandler>().To<CorrectPairSelectedEvent>();
            _dispatcher.Bind().Handler<ScoreHandler>().To<IncorrectPairSelectedEvent>();

            _dispatcher.Bind().Handler<StartRoundHandler>().To<StartRoundEvent>();
            _dispatcher.Bind().Handler<FinishRoundHandler>().To<FinishRoundEvent>();
            _dispatcher.Bind().Handler<CorrectPairSelectedHandler>().To<CorrectPairSelectedEvent>();

            _dispatcher.Bind().Handler<TimerHandler>().To<StartRoundEvent>();
            _dispatcher.Bind().Handler<TimerHandler>().To<FinishRoundEvent>();
            _dispatcher.Bind().Handler<TimerHandler>().To<PauseEvent>();
            _dispatcher.Bind().Handler<TimerHandler>().To<ResumeEvent>();
        }
    }
}