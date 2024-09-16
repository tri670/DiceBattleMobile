using Dice;
using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Characters/Enemy", order = 2)]
    public class EnemySO : ScriptableObject
    {
        public float currentHealthPoints;
        public float maxHealthPoints = 100;
        public DiceSO[] diceSet;
        public Sprite icon;
    }
}
