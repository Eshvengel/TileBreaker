using Assets.Scripts.Data.Levels;
using Assets.Scripts.Gameplay;
using Assets.Scripts.Gameplay.Field.FieldBuilder;
using Assets.Scripts.Gameplay.Handlers.PlayerActions.Implementations;
using Assets.Scripts.Gameplay.Tiles;
using Assets.Scripts.ThirdParty;
using Assets.Scripts.ThirdParty.Events;
using Gameplay;
using ThirdParty.Events;
using UnityEngine;

namespace Assets.Scripts.Core.Managers
{
    public class GamePlayManager
    {
        public GameplayState State { get; private set; }

        private Level _level;
        private Player _player;
        private IGameFieldBuilder _gameFieldBuilder;
        private readonly ILevelRepositoryReader _levelsRepository;

        public GamePlayManager(ILevelRepositoryReader levelsRepository)
        {
            _levelsRepository = levelsRepository;
            
            CreateGameFieldBuilder();
            AddListeners();
        }

        public void Dispose()
        {
            RemoveListeners();
        }

        private void Restart()
        {
            State = GameplayState.Restart;

            Rebuild();
            CreatePlayer();
        }
        
        private void CreateGameFieldBuilder()
        {
            var gameFieldBuilderGameObject = new GameObject("GameFieldBuilder");
            _gameFieldBuilder = gameFieldBuilderGameObject.AddComponent<GameFieldBuilder>();
        }

        private void Build(int levelId)
        {
            _level = _levelsRepository.Load(levelId);
            
            _gameFieldBuilder.Build(_level);
        }

        private void Rebuild()
        {
            _gameFieldBuilder.Rebuild();
        }
        
        private void CreatePlayer()
        {
            if (_player != null)
            {
                _player.Destroy();
            }
            
            _player = References.CreatePlayer();
            _player.Initialize(_gameFieldBuilder.GameField);
        }

        private void OnComplete()
        {
            State = GameplayState.Complete;

            Build(_level.Id + 1);
            CreatePlayer();
        }

        private void OnFail()
        {
            Restart();
        }

        private void AddListeners()
        {
            EventManager.AddListener<StartBuildLevelEvent>(OnStartBuildLevel);
            EventManager.AddListener<GamePlayRestartEvent>(OnRestartLevel);
            EventManager.AddListener<GamePlayStartEvent>(OnGamePlayStart);
            EventManager.AddListener<GamePlayPauseEvent>(OnGamePlayPause);
            EventManager.AddListener<GamePlayExitEvent>(OnGamePlayExit);
            EventManager.AddListener<PlayerActionCompleteEvent>(OnPlayerActionComplete);
        }

        private void OnGamePlayExit(GamePlayExitEvent e)
        {
            State = GameplayState.Complete;
            
            _gameFieldBuilder.GameField.Clear();
        }

        private void RemoveListeners()
        {
            EventManager.RemoveListener<StartBuildLevelEvent>(OnStartBuildLevel);
            EventManager.RemoveListener<GamePlayRestartEvent>(OnRestartLevel);
            EventManager.RemoveListener<GamePlayStartEvent>(OnGamePlayStart);
            EventManager.RemoveListener<GamePlayPauseEvent>(OnGamePlayPause);
            EventManager.RemoveListener<GamePlayExitEvent>(OnGamePlayExit);
            EventManager.RemoveListener<PlayerActionCompleteEvent>(OnPlayerActionComplete);
        }

        private void OnStartBuildLevel(StartBuildLevelEvent e)
        {
            if (State == GameplayState.Play) return;

            Build(e.LevelId);
            CreatePlayer();
        }
        
        private void OnRestartLevel(GamePlayRestartEvent e)
        {
            Restart();
        }

        private void OnGamePlayStart(GamePlayStartEvent e)
        {
            State = GameplayState.Play;
        }

        private void OnGamePlayPause(GamePlayPauseEvent e)
        {
            State = GameplayState.Pause;
        }

        private void OnPlayerActionComplete(PlayerActionCompleteEvent e)
        {
            if (!_gameFieldBuilder.GameField.HasMoves(e.Player) /*&& !e.Player.InProcess()*/ )
            {
                if (_gameFieldBuilder.GameField.IsClean())
                {
                    OnComplete();
                }
                else
                {
                    OnFail();
                }
            }
        }
    }
}
