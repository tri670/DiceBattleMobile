using System;
using UnityEngine;

namespace Dice
{
    [CreateAssetMenu(fileName = "DiceFace", menuName = "Dice/DiceFace", order = 2)]
    public class DiceFaceSO : ScriptableObject
    {
        public int damagePoint;
        public int healPoint;
        public int armorPoints;
        public int poisonPoints;

        public bool isAttack;
        public bool isHeal;
        public bool isArmorUp;
        public bool isPoison;

        [SerializeField] private String name;
    }
}