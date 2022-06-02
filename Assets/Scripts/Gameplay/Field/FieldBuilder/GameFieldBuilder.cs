using System;
using Assets.Scripts.Data.Levels;
using Assets.Scripts.Gameplay.Field.FieldPresenter;
using Assets.Scripts.Gameplay.Tiles;
using Assets.Scripts.ThirdParty;
using Assets.Scripts.ThirdParty.Events;
using Gameplay;
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

            ClearGameField();
            FillGameField();
            StartPresent(() =>
            {
                EventManager.TriggerEvent(new GamePlayStartEvent(_gameField, _level.Id));
            });
        }

        public void Rebuild()
        {
            Build(_level);
        }

        private void ClearGameField()
        {
            _gameField.Clear();
        }

        private void FillGameField()
        {
            foreach (var data in _level.Data)
            {
                var tile = References.Create(data, _gameField);
                _gameField.Add(tile);
            }
        }

        private void StartPresent(Action callback)
        {
            StartCoroutine(_presenter.Present(_gameField, callback));
        }
    }
}
