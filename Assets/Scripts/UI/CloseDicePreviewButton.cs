using GameControl;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CloseDicePreviewButton : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        private Button _button;

        void Start()
        {
            _button = gameObject.GetComponent<Button>();

            if (_button != null)
            {
                _button.onClick.AddListener(OnButtonClick);
            }
        }

        void OnButtonClick()
        {
            gameController.HideDicePreviewPanel();
        }
    }
}