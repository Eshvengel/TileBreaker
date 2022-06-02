using System.Collections;
using Assets.Scripts.Core.Managers;
using Assets.Scripts.Data.Levels;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Screens.Implementations;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    public class Game : MonoBehaviour
    {
        public const int MAX_LEVEL = 10;
        
        private static Game instance;

        public static ILevelRepositoryReader Levels => instance._levels;
        public static GamePlayManager PlayManager => instance._playManager;
        
        private GameHud _hud;
        private GamePlayManager _playManager;
        private WindowsManager _windowsManager;
        private ILevelRepositoryReader _levels;
        
        private void Awake()
        {
            Initialize();   
        }

        private void Initialize()
        {
            instance = this;
            
            StartCoroutine(InitializeCoroutine());
        }
        
        private IEnumerator InitializeCoroutine()
        {
            Preloader.OnProgress(0);
            
            _hud = GetComponent<GameHud>();
            _levels = new LevelsRepository();
            _playManager = new GamePlayManager(_levels);
            _windowsManager = GetComponent<WindowsManager>();
            
            Preloader.OnProgress(50);

            yield return _levels.LoadAllLevels();
            
            DOTween.SetTweensCapacity(200, 200);
            Application.targetFrameRate = 60;
            
            Preloader.OnProgress(100);
        }

        private void OnDestroy()
        {
            _playManager?.Dispose();
        }
    }
}
