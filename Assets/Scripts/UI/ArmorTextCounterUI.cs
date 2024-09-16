using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class ArmorTextCounterUI : MonoBehaviour
    {
        [SerializeField] private Character.Character character;
        [SerializeField] private TMP_Text text;


        void Start()
        {
            character.ArmorChanged += UpdateText;

            text.SetText(character.ArmorPoints.ToString());
        }

        private void UpdateText()
        {
            text.SetText(character.ArmorPoints.ToString());
        }

        private void OnDestroy()
        {
            character.ArmorChanged -= UpdateText;
        }
    }
}