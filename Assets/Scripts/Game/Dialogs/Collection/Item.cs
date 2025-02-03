using FrostLib.Signals.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Dialogs.Collection
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameLabel;
        [SerializeField] private TMP_Text _costLabel;
        [SerializeField] private Image _pose;
        [SerializeField] private GameObject _costContainer;    
        
        public Signal<Item> OnItemClicked = new();

        public string Name { get; private set; }

        public void Setup(string name, int cost, Sprite icon, bool isBought)
        {
            Name = name;

            _nameLabel.text = name;
            _costLabel.text = cost.ToString();
            _pose.sprite = icon;

            SetActiveCostContainer(isBought);
        }

        public void SetActiveCostContainer(bool isOn)
        {
            _costContainer.SetActive(isOn);
        }
        
        public void Select() => OnItemClicked.Dispatch(this);
    }
}
