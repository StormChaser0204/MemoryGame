using System;
using System.Linq;
using UnityEngine;

namespace Common.Cards
{
    [CreateAssetMenu(fileName = "CatsData", menuName = "Game/Data/CatsData", order = 1)]
    public class CardsData : ScriptableObject
    {
        public Info[] CatsInfo;

        public Info[] PickRandom(int amount)
        {
            return CatsInfo.OrderBy(_ => Guid.NewGuid()).Take(amount).ToArray();
        }

        public Info GetByName(string cardName) => CatsInfo.First(c => c.Name == cardName);
    }
    
    [Serializable]
    public struct Info
    {
        public string Name;
        public Sprite[] Variants;
        
        public Sprite GetRandomPose()
        {
            var rndIdx = new System.Random().Next(Variants.Length);
            return Variants[rndIdx];
        }
    }
}