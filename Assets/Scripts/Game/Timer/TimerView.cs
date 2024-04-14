using System;
using TMPro;
using UnityEngine;

namespace Game.Timer
{
    internal class TimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;

        public void UpdateTimer(float value)
        {
            var timeSpan = TimeSpan.FromSeconds(value);
            _label.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
    }
}