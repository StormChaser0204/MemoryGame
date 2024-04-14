using TMPro;
using UnityEngine;

namespace Game.Score
{
    internal class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;

        public void UpdateScore(int newValue) => _label.text = newValue.ToString();
    }
}