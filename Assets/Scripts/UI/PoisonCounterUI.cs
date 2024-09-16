using TMPro;
using UnityEngine;

namespace UI
{
    public class PoisonCounterUI : MonoBehaviour
    {
        [SerializeField] private Character.Character character;
        [SerializeField] private TMP_Text text;

        void Start()
        {
            character.PoisonChanged += UpdateText;
            UpdateText();
        }

        private void UpdateText()
        {
            text.SetText(character.GetPoisonPoints().ToString());
        }

        private void OnDestroy()
        {
            character.PoisonChanged -= UpdateText;
        }
    }
}
