using Assets.Scripts.Gameplay.Handlers.PlayerActions.Implementations;
using Assets.Scripts.Gameplay.Tiles;
using System;
using Assets.Scripts.Gameplay.Field;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Handlers
{
    public class InputHandler
    {
        private const float SENSITIVITY = 25f; 
        
        private readonly Player _player;
        private readonly GameField _gameField;
        
        public InputHandler(Player player , GameField gameField)
        {
            _player = player;
            _gameField = gameField;
        }

        public void Update()
        {
#if PLATFORM_ANDROID
            HandleTouches();
#endif
            
#if UNITY_EDITOR
            HandleKeyboard();
#endif
        }
        private void HandleKeyboard()
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                TryMakeStep(Vector2.left);

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                TryMakeStep(Vector2.right);

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                TryMakeStep(Vector2.up);

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                TryMakeStep(Vector2.down);
        }

        private void HandleTouches()
        {
            if (Input.touchSupported && Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved && touch.deltaPosition.sqrMagnitude > SENSITIVITY)
                {
                    TryMakeStep(touch.deltaPosition);
                }
            }
        }
            
        private ITile GetTile(Vector2 direction)
        {
            var absoluteX = Math.Abs(direction.x);
            var absoluteY = Math.Abs(direction.y);

            if (absoluteX > absoluteY && direction.x > 0)
                return _gameField[_player.X + 1, _player.Y];

            if (absoluteX > absoluteY && direction.x < 0)
                return _gameField[_player.X - 1, _player.Y];

            if (absoluteY > absoluteX && direction.y > 0)
                return _gameField[_player.X, _player.Y + 1];

            if (absoluteY > absoluteX && direction.y < 0)
                return _gameField[_player.X, _player.Y - 1];

            return null; 
        }

        private void TryMakeStep(Vector2 direction)
        {
            ITile fromTile = _gameField[_player.X, _player.Y];
            ITile toTile = GetTile(direction);

            if (toTile != null)
            {
                _player.TryMakeAction(new PlayerActionMove(_player, fromTile, toTile));
            }
        }
    }
}
