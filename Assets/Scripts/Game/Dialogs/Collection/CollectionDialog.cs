using System.Collections.Generic;
using Common.Cards;
using Dependencies.ChaserLib.Dialogs;
using Dependencies.ChaserLib.ServiceLocator;
using Plugins.DataHandler;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Dialogs.Collection
{
    public class CollectionDialog : DialogBase
    {
        [SerializeField] private Item _item;
        [SerializeField] private CardsData _cardsData;
        [SerializeField] private Transform _container;
        [SerializeField] private Button _buyBtn;

        private static ServiceLocator Locator => ServiceLocator.Instance;
        private static UnlockCardsData UnlockCardsData => Locator.Get<UnlockCardsData>();

        private readonly List<Item> _instances = new();
        
        private Item _selectedItem;

        public override void Show()
        {
            foreach (var info in _cardsData.CatsInfo)
            {
                var instance = Instantiate(_item, _container);
                instance.OnItemClicked.AddListener(Select);
                var isBought = IsBought(info.Name);
                instance.Setup(info.Name, info.Cost, info.IdlePose, isBought);
                
                _instances.Add(instance);
            }

            base.Show();
        }

        private static bool IsBought(string name) => UnlockCardsData.Cards.ContainsKey(name);

        private void Select(Item item)
        {
            _selectedItem = item;
            _buyBtn.interactable = !IsBought(item.Name);
        }

        public void Buy()
        {
            UnlockCardsData.Cards.Add(_selectedItem.Name, true);
            new SaveDataCommand<UnlockCardsData>(UnlockCardsData).Execute();
        }

        public override void Hide()
        {
            foreach (var instance in _instances)
            {
                instance.OnItemClicked.ClearListeners();
            }

            _instances.Clear();

            base.Hide();
        }
    }
}