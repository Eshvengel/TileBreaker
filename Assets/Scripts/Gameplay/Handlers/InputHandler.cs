using Assets.Scripts.Gameplay.Handlers.PlayerActions.Implementations;
using Assets.Scripts.Gameplay.Tiles;
using System;
using Assets.Scripts.Gameplay.Field;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Gameplay.Handlers
{
    public class InputHandler : MonoBehaviour, IDragHandler
    {
        public static Action<Vector2> OnInput;
        
        private Player _player;
        private GameField _gameField;
        
        public void Awake()
        {
            _player = FindObjectOfType<Player>();
        }

        public void Update()
        {

            if (_gameField == null)
            {
                _gameField = Game.PlayManager.GameFieldBuilder.GameField;
            }

#if UNITY_EDITOR

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                TryMakeStep(Vector2.left);

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                TryMakeStep(Vector2.right);

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                TryMakeStep(Vector2.up);

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                TryMakeStep(Vector2.down);
#endif
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.delta != Vector2.zero) // && !_player.InProcess())
            {
                if (OnInput != null)
                {
                    OnInput(eventData.delta);
                }

//                TryMakeStep(eventData.delta);
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
