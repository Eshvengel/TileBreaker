using Assets.Scripts.ThirdParty;
using Assets.Scripts.UI.Buttons;
using ThirdParty.Events;
using UnityEngine;

namespace Assets.Scripts.UI.Windows.Implementations
{
    public class WindowPause : Window
    {
        [SerializeField]
        private Button _continueButton;
        
        [SerializeField]
        private Button _exitButton;

        protected override void AddListeners()
        {
            _continueButton.AddListener(OnContinue);
            _exitButton.AddListener(OnExit);
        }

        private void OnContinue()
        {
            Hide();
        }

        private void OnExit()
        {
            EventManager.TriggerEvent(new GamePlayExitEvent());
            
            Hide();
        }
    }
}
