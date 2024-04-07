using System;
using Common.Cats;
using Cysharp.Threading.Tasks;
using Dependencies.ChaserLib.Dialogs;
using Dependencies.ChaserLib.ServiceLocator;
using Dependencies.ChaserLib.Tasks;
using Game.Cards;
using Game.Cards.Events;
using Game.Rounds;
using Game.Rounds.Events;
using Game.Rounds.Handlers;
using Plugins.EventDispatching.Dispatcher;
using UnityEngine;

namespace Game
{
    internal class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private CardHolder _cardHolder;
        [SerializeField] private DialogsLauncher _dialogsLauncher;
        [SerializeField] private CatsData _catsData;

        private static ServiceLocator Locator => ServiceLocator.Instance;
        private IEventDispatcher _dispatcher;

        public async void Start()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f));

            _dispatcher = new EventDispatcher();
            Locator.Add(_dispatcher);
            Locator.Add<IDialogsLauncher>(_dialogsLauncher);
            Locator.Add(_cardHolder);
            Locator.Add(_catsData);

            var token = gameObject.GetCancellationTokenOnDestroy();
            Locator.Add<ICancellationTokenFactory>(new CancellationTokenFactory(token));

            SetupStatistic();
            SetupRound();

            BindHandlers();

            _dispatcher.Raise(new StartRoundEvent());
        }

        private void SetupStatistic()
        {
            var statistic = new Statistic();
            Locator.Add(statistic);
        }

        private void SetupRound()
        {
            var info = new Info();
            Locator.Add(info);
        }

        private void BindHandlers()
        {
            _dispatcher.Bind().Handler<StatisticHandler>().To<CorrectPairSelectedEvent>();
            _dispatcher.Bind().Handler<StatisticHandler>().To<IncorrectPairSelectedEvent>();

            _dispatcher.Bind().Handler<StartRoundHandler>().To<StartRoundEvent>();
            _dispatcher.Bind().Handler<FinishRoundHandler>().To<FinishRoundEvent>();
            _dispatcher.Bind().Handler<CorrectPairSelectedHandler>().To<CorrectPairSelectedEvent>();
        }
    }
}