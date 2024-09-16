namespace GameControl
{
    public enum GameStatusEnum
    {
        Idle,
        DicePlayerRolling,
        DiceEnemyRolling,
        WaitingForPlayerChoice,
        DicePlayerActioning,
        DiceEnemyActioning,
        EndRound
    }
}