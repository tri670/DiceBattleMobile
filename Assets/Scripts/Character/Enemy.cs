namespace Character
{
    public class Enemy : Character
    {
        new void Start()
        {
            base.Start();
        }

        public void UpdateToNewEnemy(EnemySO newEnemy)
        {
            CurrenHealthPoints = newEnemy.currentHealthPoints;
            currentHealthPoints = newEnemy.currentHealthPoints;
            MaxHealthPoints = newEnemy.maxHealthPoints;
            maxHealthPoints = newEnemy.maxHealthPoints;
            diceSet = newEnemy.diceSet;
            IsAlive = true;
            SetIcon(newEnemy.icon);
            ResetArmor();
            ResetPoison();
            
            OnHealthChanged();
        }
    }
}