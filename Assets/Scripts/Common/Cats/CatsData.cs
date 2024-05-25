using System;
using System.Linq;
using UnityEngine;

namespace Common.Cats
{
    [CreateAssetMenu(fileName = "CatsData", menuName = "Game/Data/CatsData", order = 1)]
    public class CatsData : ScriptableObject //TODO: Rename to "CardsData"
    {
        public Info[] CatsInfo;

        public Info[] PickRandom(int amount)
        {
            return CatsInfo.OrderBy(_ => Guid.NewGuid()).Take(amount).ToArray();
        }
    }
    
    [Serializable]
    public struct Info
    {
        public string Name;
        public Sprite[] Poses; //TODO: Rename to "Variant"
        
        public Sprite GetRandomPose()
        {
            var rndIdx = new System.Random().Next(Poses.Length);
            return Poses[rndIdx];
        }
    }
}