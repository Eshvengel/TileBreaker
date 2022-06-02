using Assets.Scripts.ThirdParty;
using Assets.Scripts.ThirdParty.Events;
using ThirdParty.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = Assets.Scripts.UI.Buttons.Button;

namespace Assets.Scripts.UI
{
    public class GameHud : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _level;

        [SerializeField] 
        private TextMeshProUGUI _score;

        [SerializeField] 
        private Button _restartLevelButton;
        
        [SerializeField] 
        private Button _pauseButton;

        private void Awake()
        {
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _restartLevelButton.AddListener(Restart);
            _pauseButton.AddListener(Pause);
            
            EventManager.AddListener<GamePlayStartEvent>(OnGamePlayStart);
        }

        private void RemoveListeners()
        {
            _restartLevelButton.RemoveListener(Restart);
            _pauseButton.AddListener(Pause);
            
            EventManager.AddListener<GamePlayStartEvent>(OnGamePlayStart);
        }

        private void OnGamePlayStart(GamePlayStartEvent e)
        {
            _level.SetText($"Level " + e.LevelId);
        }
        
        private void Restart()
        {
            EventManager.TriggerEvent(new GamePlayRestartEvent());
        }

        private void Pause()
        {
            EventManager.TriggerEvent(new GamePlayPauseEvent());
        }
    }
}
