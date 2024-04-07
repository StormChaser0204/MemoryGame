using UnityEngine;

namespace Common.Cats
{
    [CreateAssetMenu(fileName = "CatsData", menuName = "Game/Data/CatsData", order = 1)]
    public class CatsData : ScriptableObject
    {
        public Sprite[] Sprites;
    }
}