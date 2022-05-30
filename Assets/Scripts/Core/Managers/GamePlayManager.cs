using Assets.Scripts.Gameplay.Field.FieldBuilder;
using Assets.Scripts.ThirdParty;
using Assets.Scripts.ThirdParty.Events;
using DG.Tweening;
using ThirdParty.Events;
using UnityEngine;

namespace Assets.Scripts.Core.Managers
{
    public class GamePlayManager
    {
        public GameplayState State { get; private set; }
        public IGameFieldBuilder GameFieldBuilder { get; private set; }

        public GamePlayManager()
        {
            CreateGameFieldBuilder();
            AddListeners();
        }

        public void Dispose()
        {
            RemoveListeners();
        }

        public void Restart()
        {
            State = GameplayState.Restart;

            EventManager.TriggerEvent(new GamePlayRestartEvent());
            
            GameFieldBuilder.Rebuild();
        }

        private void OnComplete()
        {
            State = GameplayState.Complete;

            Restart();
        }

        private void OnFail()
        {
            Restart();
        }

        private void AddListeners()
        {
            EventManager.AddListener<StartBuildLevelEvent>(OnStartBuildLevel);
            EventManager.AddListener<GamePlayStartEvent>(OnGamePlayStart);
            EventManager.AddListener<GamePlayPauseEvent>(OnGamePlayPause);
            EventManager.AddListener<PlayerActionCompleteEvent>(OnPlayerActionComplete);
        }

        private void RemoveListeners()
        {
            EventManager.RemoveListener<StartBuildLevelEvent>(OnStartBuildLevel);
            EventManager.RemoveListener<GamePlayStartEvent>(OnGamePlayStart);
            EventManager.RemoveListener<GamePlayPauseEvent>(OnGamePlayPause);
            EventManager.RemoveListener<PlayerActionCompleteEvent>(OnPlayerActionComplete);
        }

        private void OnStartBuildLevel(StartBuildLevelEvent e)
        {
            if (State == GameplayState.Play)
                return;

            GameFieldBuilder.Build(e.Level); ;
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
            if (!GameFieldBuilder.GameField.HasMoves(e.Player) && !e.Player.InProcess())
            {
                if (GameFieldBuilder.GameField.IsClean())
                {
                    OnComplete();
                }
                else
                {
                    OnFail();
                }
            }
        }

        private void CreateGameFieldBuilder()
        {
            var gameFieldBuilder = new GameObject("GameFieldBuilder");
            GameFieldBuilder = gameFieldBuilder.AddComponent<GameFieldBuilder>();
        }
    }
}
