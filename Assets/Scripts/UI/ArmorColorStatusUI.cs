using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class ArmorColorStatusUI : MonoBehaviour
    {
        [SerializeField] private Character.Character character;
        [SerializeField] private RawImage barImage;

        private readonly Color _activeColor = Color.HSVToRGB(0.5f,0.67f,0.84f);
        private readonly Color _notActiveColor = Color.HSVToRGB(0.5f,0.15f,0.58f);
    
        void Start()
        {
            character.ArmorChanged += UpdateColor;
            UpdateColor();
        }

        private void UpdateColor()
        {
            if (character.ArmorPoints > 0)
            {
                barImage.color = _activeColor;
            }
            else
            {
                barImage.color = _notActiveColor;
            }
        }

        private void OnDestroy()
        {
            character.ArmorChanged -= UpdateColor;
        }
    }
}
