using Assets.Scripts.Gameplay.Tiles;
using Assets.Scripts.ThirdParty;
using Assets.Scripts.ThirdParty.Events;
using DG.Tweening;
using System;

namespace Assets.Scripts.Gameplay.Handlers.PlayerActions.Implementations
{
    public class PlayerActionJump : PlayerAction
    {
        private const int JUMP_COUNT = 1;
        private const float JUMP_POWER = 1;
        
        private ITile _fromTile;
        private ITile _toTile;

        private Sequence _sequence;

        public PlayerActionJump(Player player, ITile fromTile, ITile toTile, float time = 0.4f, Ease ease = Ease.OutCubic) : base(player, time, ease)
        {
            _fromTile = fromTile;
            _toTile = toTile;
        }

        public override void Execute(Action onStart = null, Action onComplete = null)
        {
            onStart?.Invoke();

            _sequence = Player.Transform
                .DOJump(_toTile.WorldPosition + Player.WorldOffset, JUMP_POWER, JUMP_COUNT, Time)
                .OnStart(() =>
                {
                    _fromTile.OnPlayerExit(Player);
                })
                .OnComplete(() =>
                {
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