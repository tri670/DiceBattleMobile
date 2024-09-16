using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Dice;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameControl
{
    public class GameController : MonoBehaviour
    {
        public PlayerCharacter playerCharacter;
        public Enemy enemy;
        public int maxRoomNumber;

        [SerializeField] private Transform rollZone;
        [SerializeField] private GameObject winPanelUI;
        [SerializeField] private GameObject loosePanelUI;
        [SerializeField] private GameObject dicePreviewPanelUI;
        [SerializeField] private float jumpForce = 2f;
        [SerializeField] private float spinForce = 160f;

        [SerializeField] private Transform dicePreviewPos;
        [SerializeField] private Transform[] playerDicePos;
        [SerializeField] private Transform[] enemyDicePos;

        [SerializeField] private EnemySO[] enemiesSet;

        private Queue<EnemySO> _enemiesQueue;
        private GameStatusEnum _gameStatus = GameStatusEnum.Idle;
        private int _roomNumber = 1;
        private DiceSO _activeDice;
        private GameObject _previewDiceClone;

        public event Action NewRoomStarted;

        void Start()
        {
            SpawnDices(playerCharacter.diceSet, playerDicePos);

            playerCharacter.CharacterDied += PlayerDied;
            enemy.CharacterDied += EnemyDied;

            _enemiesQueue = new Queue<EnemySO>(enemiesSet);
            if (_enemiesQueue.Count > 0)
            {
                enemy.UpdateToNewEnemy(_enemiesQueue.Dequeue());
                SpawnDices(enemy.diceSet, enemyDicePos);
                OnNewRoomStarted();
            }
        }

        public int GetRoomNumber()
        {
            return _roomNumber;
        }

        public GameStatusEnum GetGameStatus()
        {
            return _gameStatus;
        }

        public void EnemyDiedCall()
        {
            EnemyDied();
        }

        public void PlayerDiedCall()
        {
            PlayerDied();
        }
    
        public void SetGameStatus(GameStatusEnum newGameStatus = GameStatusEnum.Idle)
        {
            _gameStatus = newGameStatus;

            Debug.Log(_gameStatus);

            switch (_gameStatus)
            {
                case GameStatusEnum.DicePlayerRolling:
                    StartCoroutine(RollDiceSequence(playerCharacter, GameStatusEnum.DiceEnemyRolling));
                    break;
                case GameStatusEnum.DiceEnemyRolling:
                    StartCoroutine(RollDiceSequence(enemy, GameStatusEnum.WaitingForPlayerChoice));
                    break;
                case GameStatusEnum.WaitingForPlayerChoice:
                    Debug.Log("Waiting for player to choose a dice...");
                    break;
                case GameStatusEnum.DicePlayerActioning:
                    StartCoroutine(DiceActions(playerCharacter, enemy, GameStatusEnum.DiceEnemyActioning));
                    break;
                case GameStatusEnum.DiceEnemyActioning:
                    StartCoroutine(DiceActions(enemy, playerCharacter, GameStatusEnum.EndRound));
                    break;
                case GameStatusEnum.EndRound:
                    EndRound();
                    break;
                case GameStatusEnum.Idle:
                    Debug.Log("Game in Idle status now");
                    break;
            }
        }
    
        public void ResetGame()
        {
            _roomNumber = 1;
            ShuffleEnemiesSet();

            playerCharacter.IsAlive = true;
            enemy.IsAlive = true;

            DespawnDices(enemy.diceSet);

            if (_enemiesQueue.Count > 0)
            {
                enemy.UpdateToNewEnemy(_enemiesQueue.Dequeue());
                SpawnDices(enemy.diceSet, enemyDicePos);
            }

            SetGameStatus(GameStatusEnum.Idle);
            OnNewRoomStarted();

            winPanelUI.SetActive(false);
        }

        public void StartNextRoom()
        {
            _roomNumber++;
            playerCharacter.ResetArmor();
            playerCharacter.ResetPoison();
            if (_enemiesQueue.Count > 0)
            {
                enemy.UpdateToNewEnemy(_enemiesQueue.Dequeue());
                SpawnDices(enemy.diceSet, enemyDicePos);
                OnNewRoomStarted();
                SetGameStatus(GameStatusEnum.Idle);
            }
            else
            {
                ShuffleEnemiesSet();
                enemy.UpdateToNewEnemy(_enemiesQueue.Dequeue());
                SpawnDices(enemy.diceSet, enemyDicePos);
                OnNewRoomStarted();
                SetGameStatus(GameStatusEnum.Idle);
            }
        }
    
        public void SelectPlayerDice(DiceSO selectedDice)
        {
            if (_gameStatus != GameStatusEnum.WaitingForPlayerChoice)
            {
                return;
            }

            if (selectedDice != null)
            {
                _activeDice = selectedDice;
                SetGameStatus(GameStatusEnum.DicePlayerActioning);
            }
        }
    
        public void ShowDicePreviewPanel(DiceSO dice)
        {
            _previewDiceClone = Instantiate(dice.model, dicePreviewPos.position, dicePreviewPos.rotation);

            Rigidbody cloneRb = _previewDiceClone.GetComponent<Rigidbody>();
            if (cloneRb != null)
            {
                cloneRb.isKinematic = true;
            }

            dicePreviewPanelUI.SetActive(true);
        }

        public void HideDicePreviewPanel()
        {
            if (_previewDiceClone != null)
            {
                Destroy(_previewDiceClone);
            }

            dicePreviewPanelUI.SetActive(false);
        }

        private void DespawnDices(DiceSO[] diceSet)
        {
            foreach (var dice in diceSet)
            {
                if (dice.model != null)
                {
                    Destroy(dice.model);
                }
            }
        }

        private void EnemyDied()
        {
            DespawnDices(enemy.diceSet);
            StopAllCoroutines();
            _gameStatus = GameStatusEnum.Idle;

            enemy.IsAlive = false;

            if (_roomNumber >= maxRoomNumber)
            {
                winPanelUI.SetActive(true);
            }
            else
            {
                winPanelUI.SetActive(true);
            }
        }

        private void PlayerDied()
        {
            StopAllCoroutines();
            playerCharacter.IsAlive = false;
            _gameStatus = GameStatusEnum.Idle;
            playerCharacter.ResetStats();
            playerCharacter.ResetStats();

            loosePanelUI.SetActive(true);
        }
    
        private void ShuffleEnemiesSet()
        {
            EnemySO[] shuffledEnemiesSet = (EnemySO[])enemiesSet.Clone();

            int n = shuffledEnemiesSet.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);

                EnemySO temp = shuffledEnemiesSet[i];
                shuffledEnemiesSet[i] = shuffledEnemiesSet[randomIndex];
                shuffledEnemiesSet[randomIndex] = temp;
            }

            _enemiesQueue = new Queue<EnemySO>(shuffledEnemiesSet);
        }

        private void EndRound()
        {
            if (playerCharacter.IsAlive && enemy.IsAlive)
            {
                enemy.ResetArmor();
                playerCharacter.ResetArmor();
                enemy.ApplyPoisonEffect();
                playerCharacter.ApplyPoisonEffect();
                enemy.ResetPoisonEffectApplied();
                playerCharacter.ResetPoisonEffectApplied();
            }

            _gameStatus = GameStatusEnum.Idle;
        }

        private void SpawnDices(DiceSO[] diceSet, Transform[] spawnPos)
        {
            int dicePosesCounter = 0;
            foreach (var dice in diceSet)
            {
                dice.Spawn(spawnPos[dicePosesCounter], this);
                dicePosesCounter++;
            }
        }

        private void MoveDicesToRollZone(DiceSO[] diceSet)
        {
            foreach (var dice in diceSet)
            {
                dice.model.transform.position = rollZone.position + new Vector3(
                    Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            }
        }

        private void ReturnDicesToOriginalPositions(DiceSO[] diceSet, Transform[] dicePoses)
        {
            for (int i = 0; i < diceSet.Length; i++)
            {
                diceSet[i].model.transform.position = dicePoses[i].position;
                diceSet[i].model.transform.rotation = diceSet[i].GetRotationForActiveFace();
            }
        }

        private IEnumerator DiceActions(Character.Character source, Character.Character target, GameStatusEnum nextStatus)
        {
            if (!source.IsAlive || !target.IsAlive)
            {
                yield break;
            }

            if (_gameStatus == GameStatusEnum.DicePlayerActioning)
            {
                _activeDice.ApplyDiceAction(source, target);
            }
            else if (_gameStatus == GameStatusEnum.DiceEnemyActioning)
            {
                enemy.diceSet[0].ApplyDiceAction(source, target);
            }

            yield return new WaitForSeconds(0.8f);

            SetGameStatus(nextStatus);
        }

        private IEnumerator RollDiceSequence(Character.Character character, GameStatusEnum nextStatus)
        {
            if (!character.IsAlive)
            {
                yield break;
            }

            MoveDicesToRollZone(character.diceSet);
            RotateAllDices(character.diceSet);

            yield return new WaitForSeconds(3f);

            foreach (var dice in character.diceSet)
            {
                dice.DetectUpFace();
            }

            ReturnDicesToOriginalPositions(character.diceSet, character is PlayerCharacter ? playerDicePos : enemyDicePos);
            SetGameStatus(nextStatus);
        }

        private void ClampDiceVelocity(Rigidbody rb)
        {
            if (rb.velocity.y > 5f)
            {
                rb.velocity = new Vector3(rb.velocity.x, 5f, rb.velocity.z);
            }
        }

        private void RotateAllDices(DiceSO[] diceSet)
        {
            foreach (var dice in diceSet)
            {
                Vector3 randomRotation = new Vector3(
                    Random.Range(-spinForce, spinForce),
                    Random.Range(-spinForce, spinForce),
                    Random.Range(-spinForce, spinForce)
                );

                dice.Rotate(randomRotation, jumpForce);

                ClampDiceVelocity(dice.rb);
            }
        }
    
        private void OnNewRoomStarted()
        {
            NewRoomStarted?.Invoke();
        }

        private void OnDestroy()
        {
            playerCharacter.CharacterDied -= PlayerDied;
            enemy.CharacterDied -= EnemyDied;
        }
    }
}