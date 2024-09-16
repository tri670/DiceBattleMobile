using GameControl;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoosePanelUI : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private Button button;

        void Start()
        {
            if (button != null)
            {
                button.onClick.AddListener(OnButtonClick);
            }
        }

        void OnButtonClick()
        {
            gameObject.SetActive(false);
            gameController.ResetGame();
        }
    }
}