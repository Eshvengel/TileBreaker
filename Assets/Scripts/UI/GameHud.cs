using Assets.Scripts.ThirdParty;
using Assets.Scripts.ThirdParty.Events;
using Assets.Scripts.UI.Buttons;
using TMPro;
using UnityEngine;

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
        }

        private void RemoveListeners()
        {
            _restartLevelButton.RemoveListener(Restart);
        }

        private void Restart()
        {
            EventManager.TriggerEvent(new GamePlayRestartEvent());
        }
    }
}
