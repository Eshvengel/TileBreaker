using System;
using Assets.Scripts.Data.Levels;
using Assets.Scripts.Gameplay.Field.FieldPresenter;
using Assets.Scripts.Gameplay.Tiles;
using Assets.Scripts.ThirdParty;
using Assets.Scripts.ThirdParty.Events;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Field.FieldBuilder
{
    public class GameFieldBuilder : MonoBehaviour, IGameFieldBuilder
    {
        public GameField GameField => _gameField;

        private Level _level;
        private GameField _gameField;
        private IGameFieldPresenter _presenter;

        private void Awake()
        {
            _gameField = new GameField();
            _presenter = new DefaultGameFieldPresenter();

            //_presenter = new LeftDownToRightUpGameFieldPresenter();
        }

        public void Build(Level level)
        {
            if (level == null)
            {
                Debug.LogError($"Level is null.");
                return;
            }

            if (level.Data == null || level.Data.Length <= 0)
            {
                Debug.LogError($"Can't build level \"{level.Id}\" because Data is null or empty.");
                return;
            }

            _level = level;

            PrepareGameField();
            CreateTiles(level);
            StartPresent(_gameField, () =>
            {
                EventManager.TriggerEvent(new GamePlayStartEvent(_gameField));
            });
        }

        public void Rebuild()
        {
            Build(_level);
        }

        private void PrepareGameField()
        {
            _gameField.Dispose();
        }

        private void CreateTiles(Level level)
        {
            foreach (var data in level.Data)
            {
                var tile = TilesContainer.Create(data, _gameField);
                _gameField.Add(tile);
            }
        }

        private void StartPresent(GameField gameField, Action callback)
        {
            StartCoroutine(_presenter.Present(gameField, callback));
            
            //Game.Instance.StartCoroutine(_presenter.Present(gameField, callback));
        }
    }
}
