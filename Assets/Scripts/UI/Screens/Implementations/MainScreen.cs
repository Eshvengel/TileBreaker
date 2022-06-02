using System.Collections;
using Assets.Scripts.ThirdParty;
using Assets.Scripts.ThirdParty.Events;
using Assets.Scripts.UI.Buttons;
using Assets.Scripts.UI.Panels;
using ThirdParty.Events;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Implementations
{
    public class MainScreen : Screen
    {
        [SerializeField] private Panel _settingsPanel;
        [SerializeField] private Panel _levelsPanel;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _levelsButton;

        public void OnEnable()
        {
            AddListeners();
        }

        public void OnDisable()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _playButton.AddListener(Play);
            _levelsButton.AddListener(Levels);
            _settingsButton.AddListener(Settings);
            
            EventManager.AddListener<GamePlayExitEvent>(OnExit);
        }
        
        private void RemoveListeners()
        {
            _playButton.RemoveListener(Play);
            _levelsButton.RemoveListener(Levels);
            _settingsButton.RemoveListener(Settings);
            
            EventManager.RemoveListener<GamePlayExitEvent>(OnExit);
        }

        private void OnExit(GamePlayExitEvent e)
        {
            SetActive(true);
        }

        private void Play()
        {
            // TODO: Remove "1" and get level ID from some storage.

            EventManager.TriggerEvent(new StartBuildLevelEvent(1));

            SetActive(false);
        }

        private void Settings()
        {
            _settingsPanel.Show();
        }

        private void Levels()
        {
            _levelsPanel.Show();
        }
    }
}
