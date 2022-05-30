using Assets.Scripts.Gameplay.Tiles;
using Assets.Scripts.ThirdParty;
using Assets.Scripts.ThirdParty.Events;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Handlers.PlayerActions.Implementations
{
    public class PlayerActionMove : PlayerAction
    {
        private const int ROTATE_ANGLE = 90;

        private ITile _fromTile;
        private ITile _toTile;

        public PlayerActionMove(Player player, ITile fromTile, ITile toTile, float time = 0.15f, Ease ease = Ease.OutSine) : base(player, time, ease)
        {
            _fromTile = fromTile;
            _toTile = toTile;
        }

        public override void Execute(Action onStart = null, Action onComplete = null)
        {
            onStart?.Invoke();

            var lastValue = 0f;
            var direction = CalcDirection(Player, _toTile);
            var rotatePoint = CalcRotatePoint(direction);
            var rotateAxis = CalcRotateAxis(direction);
            
            DOVirtual
                .Float(0, 1, Time, value =>
                {
                    var calcAngle = ROTATE_ANGLE * (value - lastValue);
                    lastValue = value;

                    Player.Transform.RotateAround(rotatePoint, rotateAxis, calcAngle);
                })
                .OnStart(() =>
                {
                    _fromTile.OnPlayerExit(Player);
                })
                .OnComplete(() =>
                {
                    Player.RoundRotation();
                    Player.SetPosition(_toTile);

                    onComplete?.Invoke();

                    _toTile.OnPlayerEnter(Player);

                    EventManager.TriggerEvent(new PlayerActionCompleteEvent(Player));
                })
                .SetEase(Ease);
        }

        public override bool CanExecute()
        {
            return _toTile != null && _toTile.Interactable;
        }

        private Vector3 CalcDirection(Player player, ITile toTile)
        {
            return new Vector3(toTile.WorldPosition.x - player.WorldPosition.x, 0, toTile.WorldPosition.z - player.WorldPosition.z).normalized;
        }

        private Vector3 CalcRotatePoint(Vector3 direction)
        {
            return new Vector3(Player.WorldPosition.x + Player.MeshRenderer.bounds.size.x / 2 * direction.x,
                                Player.WorldPosition.y - Player.MeshRenderer.bounds.size.y / 2,
                                Player.WorldPosition.z + Player.MeshRenderer.bounds.size.z / 2 * direction.z);
        }

        private Vector3 CalcRotateAxis(Vector3 direction)
        {
            return Vector3.Cross(Vector3.up, direction);
        }
    }
}