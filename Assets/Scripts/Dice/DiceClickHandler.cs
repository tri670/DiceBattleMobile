using System.Collections;
using GameControl;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dice
{
    public class DiceClickHandler : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        private readonly float _pressTimeThreshold = 0.5f;
        private DiceSO _dice;
        private GameController _gameController;
        private bool _isPressed;
        private float _pressStartTime;
        private Coroutine _holdCoroutine;

        public void Initialize(DiceSO dice, GameController gameController)
        {
            _dice = dice;
            _gameController = gameController;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
            _pressStartTime = Time.time;
            _holdCoroutine = StartCoroutine(CheckForLongPress());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
            float pressDuration = Time.time - _pressStartTime;

            if (_holdCoroutine != null)
            {
                StopCoroutine(_holdCoroutine);
            }

            if (pressDuration < _pressTimeThreshold)
            {
                if (_gameController.GetGameStatus() == GameStatusEnum.WaitingForPlayerChoice)
                {
                    _gameController.SelectPlayerDice(_dice);
                }
            }
        }

        private IEnumerator CheckForLongPress()
        {
            yield return new WaitForSeconds(_pressTimeThreshold);

            if (_isPressed)
            {
                _gameController.ShowDicePreviewPanel(_dice);
            }
        }
    }
}