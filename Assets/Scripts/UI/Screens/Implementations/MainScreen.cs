using System.Collections;
using Assets.Scripts.ThirdParty;
using Assets.Scripts.ThirdParty.Events;
using Assets.Scripts.UI.Buttons;
using Assets.Scripts.UI.Panels;
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

        public void Start()
        {
            AddListeners();
        }

        public void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _playButton.AddListener(Play);
            _levelsButton.AddListener(Levels);
            _settingsButton.AddListener(Settings);
        }

        private void RemoveListeners()
        {
            _playButton.RemoveListener(Play);
            _levelsButton.RemoveListener(Levels);
            _settingsButton.RemoveListener(Settings);
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
