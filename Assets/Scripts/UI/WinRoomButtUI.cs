using GameControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WinRoomButtUI : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text roundCountTxt;
        [SerializeField] private TMP_Text buttonTxt;

        void Start()
        {
            if (button != null)
            {
                button.onClick.AddListener(OnButtonClick);
            }

            gameController.NewRoomStarted += NewRoomStart;

            UpdateRoomCountTxt();
        }

        private void NewRoomStart()
        {
            if (gameController.GetRoomNumber() >= gameController.maxRoomNumber)
            {
                buttonTxt.SetText("Restart game");
            }
            else
            {
                buttonTxt.SetText("Next Room");
            }

            UpdateRoomCountTxt();
        }

        private void UpdateRoomCountTxt()
        {
            roundCountTxt.SetText("Room: " + gameController.GetRoomNumber().ToString() + "/" +
                                  gameController.maxRoomNumber);
        }

        private void OnDestroy()
        {
            gameController.NewRoomStarted -= NewRoomStart;
        }

        void OnButtonClick()
        {
            if (gameController.GetRoomNumber() >= gameController.maxRoomNumber)
            {
                gameObject.SetActive(false);
                gameController.ResetGame();
            }
            else
            {
                gameObject.SetActive(false);
                gameController.StartNextRoom();
            }
        }
    }
}