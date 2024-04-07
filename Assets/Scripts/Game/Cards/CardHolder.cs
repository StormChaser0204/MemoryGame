using System;
using System.Collections.Generic;
using System.Linq;
using Common.Cats;
using Cysharp.Threading.Tasks;
using Dependencies.ChaserLib.ServiceLocator;
using Game.Cards.Events;
using Plugins.EventDispatching.Dispatcher;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Cards
{
    internal class CardHolder : MonoBehaviour, IDisposable
    {
        [SerializeField] private Card _prefab;
        [SerializeField] private Transform _container;
        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private CatsData _catsData;

        [SerializeField] private Vector2 _threeColumnsSize;
        [SerializeField] private Vector2 _fourColumnsSize;

        private static ServiceLocator Locator => ServiceLocator.Instance;
        private static IEventDispatcher Dispatcher => Locator.Get<IEventDispatcher>();

        private readonly List<Card> _instances = new();

        private Card _selectedCard;

        public async UniTask SetItems(List<int> values)
        {
            SetupGrid(values.Count);
            var delay = TimeSpan.FromSeconds(0.15f);

            var shufled = _catsData.Sprites.OrderBy(_ => Guid.NewGuid()).ToList();

            foreach (var value in values)
            {
                var inst = Instantiate(_prefab, _container);
                inst.Init(value, shufled[value]);
                inst.OnSelected.AddListener(SelectCard);
                _instances.Add(inst);
            }

            foreach (var instance in _instances)
            {
                instance.ShowCard();
                await UniTask.Delay(delay);
            }
        }

        private void SetupGrid(int amount)
        {
            var isThreeColumns = amount <= 12;

            _grid.constraintCount = isThreeColumns ? 3 : 4;
            _grid.cellSize = isThreeColumns ? _threeColumnsSize : _fourColumnsSize;
        }

        private async void SelectCard(Card card)
        {
            if (_selectedCard == null)
            {
                _selectedCard = card;
                await _selectedCard.Show();
                return;
            }

            if (_selectedCard == card)
            {
                card.Hide();
                _selectedCard = null;
                return;
            }

            var selectedCard = _selectedCard;
            _selectedCard = null;
            await card.Show();

            var cardId = card.GetId();
            var isSameValue = selectedCard.GetId() == cardId;
            if (isSameValue)
            {
                Dispatcher.Raise(new CorrectPairSelectedEvent());
                selectedCard.RemoveCard();
                card.RemoveCard();
            }
            else
            {
                Dispatcher.Raise(new IncorrectPairSelectedEvent());
                selectedCard.Hide();
                card.Hide();
            }

        }

        public void OnDestroy()
        {
            foreach (var inst in _instances)
            {
                inst.OnSelected.RemoveListener(SelectCard);
            }
        }

        public void Dispose()
        {
            foreach (var instance in _instances)
                instance.Dispose();

            _instances.Clear();
        }
    }
}