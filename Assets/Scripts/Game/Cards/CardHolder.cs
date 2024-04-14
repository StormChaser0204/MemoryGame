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
        [Serializable]
        private struct GridConfig
        {
            public Vector2 CellSize;
            public Card Prefab;
        }

        [SerializeField] private Transform _container;
        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private CatsData _catsData;

        [SerializeField] private GridConfig _threeColumnsConfig;
        [SerializeField] private GridConfig _fourColumnsConfig;

        private static ServiceLocator Locator => ServiceLocator.Instance;
        private static IEventDispatcher Dispatcher => Locator.Get<IEventDispatcher>();

        private readonly List<Card> _instances = new();

        private Card _selectedCard;

        public async UniTask SetItems(List<int> values)
        {
            var isThreeColumns = values.Count <= 12;
            var config = isThreeColumns ? _threeColumnsConfig : _fourColumnsConfig;
            var columnsCount = isThreeColumns ? 3 : 4;
            
            SetupGrid(columnsCount, config.CellSize);
            var delay = TimeSpan.FromSeconds(0.15f);

            var shufled = _catsData.Sprites.OrderBy(_ => Guid.NewGuid()).ToList();
            var prefab = config.Prefab;

            foreach (var value in values)
            {
                var inst = Instantiate(prefab, _container);
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

        private void SetupGrid(int constaintCount, Vector2 cellSize)
        {
            _grid.constraintCount = constaintCount;
            _grid.cellSize = cellSize;
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