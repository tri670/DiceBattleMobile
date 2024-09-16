using TMPro;
using UnityEngine;

namespace UI
{
    public class HealthTextCounterUI : MonoBehaviour
    {
        [SerializeField] private Character.Character character;
        public TMP_Text text;

    
        void Start()
        {
            character.HealthChanged += UpdateText;
        
            text.SetText(character.CurrenHealthPoints + "/" + character.MaxHealthPoints);
        }

        private void UpdateText(float healthPercentage)
        {
            text.SetText(character.CurrenHealthPoints + "/" + character.MaxHealthPoints);
        }

        private void OnDestroy()
        {
            character.HealthChanged -= UpdateText;
        }
    }
}