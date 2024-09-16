using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingsUI
{
    public class OpenSettingsButton : MonoBehaviour
    {
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private Button button;
        void Start()
        {
            button = gameObject.GetComponent<Button>();

            if (button != null)
            {
                button.onClick.AddListener(OnButtonClick);
            }
        }

        void OnButtonClick()
        {
            if(!settingsPanel.activeSelf) settingsPanel.SetActive(true);
        }
    }
}
