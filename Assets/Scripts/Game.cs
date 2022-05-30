using Assets.Scripts.Core.Managers;
using Assets.Scripts.Data.Levels;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Screens.Implementations;
using UnityEngine;

namespace Assets.Scripts
{
    public class Game : MonoBehaviour
    {
        #region Singletone

        public static Game Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<Game>();

                return instance;
            }
        }
        private static Game instance;

        #endregion

        public static LevelsRepository Levels => Instance._levels;
        public static GamePlayManager PlayManager => Instance._playManager;
        
        private GameHud _hud;
        private LevelsRepository _levels;
        private WindowsManager _windowsManager;
        private GamePlayManager _playManager;
            
        private void Awake()
        {
            _levels = new LevelsRepository();
            _playManager = new GamePlayManager();
            _hud = GetComponent<GameHud>();
            _windowsManager = GetComponent<WindowsManager>();

            Preloader.OnProgress(100);
        }

        private void OnDestroy()
        {
            _playManager.Dispose();
        }
    }
}
