using GameControl;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingsUI
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private Button buttonClose;
        [SerializeField] private Button buttonNextRoom;
        [SerializeField] private Button buttonKillPlayer;
        [SerializeField] private Button buttonAddPlayerHp;
        [SerializeField] private Button buttonAddEnemyHp;
        [SerializeField] private Button buttonAddPlayerArmor;
        [SerializeField] private Button buttonAddEnemyArmor;
        [SerializeField] private Button buttonAddPlayerPoison;
        [SerializeField] private Button buttonAddEnemyPoison;
        [SerializeField] private Button buttonDamageEnemy;
        [SerializeField] private Button buttonDamagePlayer;

        void Start()
        {
            buttonClose.onClick.AddListener(Close);
            buttonNextRoom.onClick.AddListener(NextRoom);
            buttonKillPlayer.onClick.AddListener(KillPlayer);
            buttonAddPlayerHp.onClick.AddListener(AddPlayerHP);
            buttonAddEnemyHp.onClick.AddListener(AddEnemyHP);
            buttonAddPlayerArmor.onClick.AddListener(AddPlayerArmor);
            buttonAddEnemyArmor.onClick.AddListener(AddEnemyArmor);
            buttonAddPlayerPoison.onClick.AddListener(AddPlayerPoison);
            buttonAddEnemyPoison.onClick.AddListener(AddEnemyPoison);
            buttonDamageEnemy.onClick.AddListener(DamageEnemy);
            buttonDamagePlayer.onClick.AddListener(DamagePlayer);
        }

        private void Close()
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
        }

        private void NextRoom()
        {
            gameController.EnemyDiedCall();
        }

        private void KillPlayer()
        {
            gameController.PlayerDiedCall();
        }

        private void AddPlayerHP()
        {
            gameController.playerCharacter.RestoreHealth(10);
        }

        private void AddEnemyHP()
        {
            gameController.enemy.RestoreHealth(10);
        }

        private void AddPlayerArmor()
        {
            gameController.playerCharacter.IncreaseArmor(1);
        }

        private void AddEnemyArmor()
        {
            gameController.enemy.IncreaseArmor(1);
        }

        private void AddPlayerPoison()
        {
            gameController.playerCharacter.AddPoisonPoints(1);
        }

        private void AddEnemyPoison()
        {
            gameController.enemy.AddPoisonPoints(1);
        }

        private void DamageEnemy()
        {
            gameController.enemy.GetDamage(5);
        }

        private void DamagePlayer()
        {
            gameController.playerCharacter.GetDamage(5);
        }
    }
}