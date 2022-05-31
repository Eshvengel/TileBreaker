using System;
using Assets.Scripts.Gameplay.Tiles;
using Assets.Scripts.ThirdParty;
using Assets.Scripts.ThirdParty.Events;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Handlers.PlayerActions.Implementations
{
    public class PlayerActionSpawn : PlayerAction
    {
        private readonly ITile _toTile;
        private Sequence _sequence;
        public PlayerActionSpawn(Player player, ITile toTile, float time = .5f, Ease ease = Ease.OutSine) : base(player, time, ease)
        {
            _toTile = toTile;
        }

        public override void Execute(Action onStart = null, Action onComplete = null)
        {
            onStart?.Invoke();

            _sequence = DOTween.Sequence();

            _sequence
                .Append(Player.Transform.DOScale(Vector3.one, Time))
                .Join(Player.Transform.DOMoveY(_toTile.WorldPosition.y + Player.WorldOffset.y, Time))
                .OnStart(() =>
                {
                    Player.WorldPosition = _toTile.WorldPosition + Vector3.up * 5f;
                    Player.Transform.localScale = Vector3.zero;
                })
                .OnComplete(() =>
                {
                    Player.SetPosition(_toTile);
                    Player.RoundRotation();
                    
                    onComplete?.Invoke();
                    
                    _toTile.OnPlayerEnter(Player);

                    EventManager.TriggerEvent(new PlayerActionCompleteEvent(Player));
                });
        }

        public override bool CanExecute()
        {
            return _toTile != null && _toTile.Interactable;
        }

        public override void Dispose()
        {
            if (_sequence != null)
            {
                _sequence.Kill();
                _sequence = null;
            }
        }
    }
}