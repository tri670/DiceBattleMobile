using GameControl;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UpdateCharacterIconUI : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private Image image;
        [SerializeField] private bool isPlayerCharacter;
    
        void Start()
        {
            gameController.NewRoomStarted += UpdateIcon;
        
            if (isPlayerCharacter)
            {
                image.sprite = gameController.playerCharacter.GetIcon();
            }
            UpdateIcon();
        }
        private void UpdateIcon()
        {
            if (!isPlayerCharacter)
            {
                image.sprite = gameController.enemy.GetIcon();
            }
        }
    }
}
