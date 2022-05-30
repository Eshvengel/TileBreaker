using System.Linq;
using Assets.Scripts.Gameplay;
using Assets.Scripts.UI.Windows;
using Assets.Scripts.UI.Windows.Implementations;
using UnityEngine;

namespace Assets.Scripts.Core.Managers
{
    public class WindowsManager : MonoBehaviour
    {
        [SerializeField]
        private Window[] _windows;

       public void Open<T>() where T : Window
        {
            var window = _windows.FirstOrDefault(w => w is T);

            if (window != null)
            {
                window.Open();
            }
            
        }

        public void OnEscape()
        {
            var gameState = Game.PlayManager.State;

            if (gameState == GameplayState.Play)
            {
                OpenWindowOnEscape<WindowPause>();
            }
            else
            {
                OpenWindowOnEscape<WindowExit>();
            }
        }

        private void OpenWindowOnEscape<T>() where T : Window
        {
            var lastOpenWindow = _windows.OrderBy(window => window.transform.GetSiblingIndex()).FirstOrDefault(window => window.State == WindowState.Open);

            if (lastOpenWindow != null)
            {
                lastOpenWindow.Hide();
            }
            else
            {
                Open<T>();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnEscape();
            }
        }
    }
}
