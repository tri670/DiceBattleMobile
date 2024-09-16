using GameControl;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RollButtonController : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private GameController gameController;
        [SerializeField] private ParticleSystem rollZoneEffect;
        [SerializeField] private ParticleSystem cubeZoneEffect;

        void Start()
        {
            if (button != null)
            {
                button.onClick.AddListener(OnButtonClick);
            }
        }

        private void Update()
        {
            if (gameController.GetGameStatus() == GameStatusEnum.Idle)
            {
                if (!rollZoneEffect.isPlaying)
                {
                    rollZoneEffect.Play();
                }
            }
            else
            {
                if (rollZoneEffect.isPlaying)
                {
                    rollZoneEffect.Stop();
                }
            }

            if (gameController.GetGameStatus() == GameStatusEnum.WaitingForPlayerChoice)
            {
                if (!cubeZoneEffect.isPlaying)
                {
                    cubeZoneEffect.Play();
                }
            }
            else
            {
                if (cubeZoneEffect.isPlaying)
                {
                    cubeZoneEffect.Stop();
                }
            }
        }

        void OnButtonClick()
        {
            if (gameController.GetGameStatus() == GameStatusEnum.Idle)
            {
                gameController.SetGameStatus(GameStatusEnum.DicePlayerRolling);
            }
        }
    }
}