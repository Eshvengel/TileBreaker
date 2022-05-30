using Assets.Scripts.UI.Buttons;
using UnityEngine;

namespace Assets.Scripts.UI.Windows.Implementations
{
    public class WindowExit : Window
    {
        [SerializeField]
        private Button _confirmButton;

        [SerializeField]
        private Button _rejectButton;

        protected override void AddListeners()
        {
            _confirmButton.AddListener(OnConfirm);
            _rejectButton.AddListener(OnReject);
        }

        private void OnConfirm()
        {
            Application.Quit();
        }

        private void OnReject()
        {
            Hide();
        }
    }
}
