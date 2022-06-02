using System.Linq;
using Assets.Scripts.ThirdParty;
using Assets.Scripts.UI.Windows;
using Assets.Scripts.UI.Windows.Implementations;
using ThirdParty.Events;
using UnityEngine;

namespace Assets.Scripts.Core.Managers
{
    public class WindowsManager : MonoBehaviour
    {
        [SerializeField]
        private Window[] _windows;

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            EventManager.AddListener<GamePlayPauseEvent>(OnPauseEvent);
        }

        private void RemoveListeners()
        {
            EventManager.RemoveListener<GamePlayPauseEvent>(OnPauseEvent);
        }

        private void OnPauseEvent(GamePlayPauseEvent e)
        {
           Open<WindowPause>();
        }

        public void Open<T>() where T : Window
        {
            var window = _windows.FirstOrDefault(w => w is T);

            if (window != null)
            {
                window.Open();
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
                OpenWindowOnEscape<WindowExit>();
            }
        }
    }
}
