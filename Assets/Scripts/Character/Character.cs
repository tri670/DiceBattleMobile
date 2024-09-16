using System;
using Dice;
using UnityEngine;

namespace Character
{
    public class Character : MonoBehaviour
    {
        public DiceSO[] diceSet;

        [SerializeField] protected float currentHealthPoints;
        [SerializeField] protected float maxHealthPoints = 100;
        
        
        [SerializeField] private int armorPoints = 0;
        [SerializeField] private int poisoningPoints = 0;
        [SerializeField] private ParticleSystem damageEffect;
        [SerializeField] private ParticleSystem healEffect;
        [SerializeField] private ParticleSystem armorEffect;
        [SerializeField] private ParticleSystem poisonEffect;
        private bool _isPoisonEffectApplied = false;
        private Sprite _icon;

        public float CurrenHealthPoints { get; set; }
        public float MaxHealthPoints { get; set; }
        public int ArmorPoints { get; set; }
        public bool IsAlive { get; set; } = true;

        public event Action<float> HealthChanged;
        public event Action ArmorChanged;
        public event Action PoisonChanged;
        public event Action CharacterDied;

        public void SetIcon(Sprite icon)
        {
            _icon = icon;
        }

        public Sprite GetIcon()
        {
            return _icon;
        }

        public int GetPoisonPoints()
        {
            return poisoningPoints;
        }

        public void ResetArmor()
        {
            ArmorPoints = 0;
            armorPoints = 0;
            OnArmorChanged();
        }
        
        public void ResetPoison()
        {
            poisoningPoints = 0;
            OnPoisonChanged();
        }

        public void Die()
        {
            IsAlive = false;
            OnCharacterDied();
        }

        public void GetDamage(int damage)
        {
            if (armorPoints > 0)
            {
                if (damage > armorPoints)
                {
                    float remainingDamage = damage - armorPoints;
                    armorPoints = 0;
                    currentHealthPoints -= remainingDamage;
                    damageEffect.Play();
                }
                else
                {
                    armorPoints -= damage;
                }

                ArmorPoints = armorPoints;
                OnArmorChanged();
            }
            else
            {
                currentHealthPoints -= damage;
                damageEffect.Play();
            }

            CurrenHealthPoints = Mathf.Clamp(CurrenHealthPoints, 0, MaxHealthPoints);
            CurrenHealthPoints = currentHealthPoints;
            OnHealthChanged();

            if (currentHealthPoints <= 0) Death();
        }


        public void ResetPoisonEffectApplied()
        {
            _isPoisonEffectApplied = false;
        }

        public void AddPoisonPoints(int poisonPoints)
        {
            if (poisoningPoints <= 0)
            {
                _isPoisonEffectApplied = true;
            }

            poisoningPoints += poisonPoints;
            OnPoisonChanged();
            poisonEffect.Play();
        }

        public void ApplyPoisonEffect()
        {
            if (!_isPoisonEffectApplied && poisoningPoints > 0)
            {
                GetDamage(poisoningPoints);
                _isPoisonEffectApplied = true;
                poisoningPoints--;
                OnPoisonChanged();
            }
        }

        public void IncreaseArmor(int newArmorPoints)
        {
            armorPoints += newArmorPoints;
            ArmorPoints = armorPoints;
            OnArmorChanged();
            armorEffect.Play();
        }

        public void Death()
        {
            OnCharacterDied();
        }

        public void RestoreHealth(int restorePoints)
        {
            float newCurrentHealth = restorePoints + currentHealthPoints;

            if (newCurrentHealth <= maxHealthPoints) currentHealthPoints = newCurrentHealth;
            else currentHealthPoints = maxHealthPoints;

            CurrenHealthPoints = Mathf.Clamp(CurrenHealthPoints, 0, MaxHealthPoints);
            CurrenHealthPoints = currentHealthPoints;
            OnHealthChanged();
            healEffect.Play();
        }

        protected void Start()
        {
            CurrenHealthPoints = currentHealthPoints;
            MaxHealthPoints = maxHealthPoints;
            OnHealthChanged();
        }


        protected void OnHealthChanged()
        {
            HealthChanged?.Invoke(CurrenHealthPoints / MaxHealthPoints);
        }

        protected void OnArmorChanged()
        {
            ArmorChanged?.Invoke();
        }

        private void OnCharacterDied()
        {
            CharacterDied?.Invoke();
        }

        protected void OnPoisonChanged()
        {
            PoisonChanged?.Invoke();
        }
    }
}