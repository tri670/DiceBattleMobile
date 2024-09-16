namespace Character
{
    public class PlayerCharacter : Character
    {
        new void Start()
        {
            base.Start();
        }
        public void ResetStats()
        {
            currentHealthPoints = maxHealthPoints;
            CurrenHealthPoints = maxHealthPoints;
            ResetArmor();
            ResetPoison();
            
            OnHealthChanged();
        }
    }
}