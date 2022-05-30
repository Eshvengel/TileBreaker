using Assets.Scripts.Gameplay.Tiles;
using Assets.Scripts.ThirdParty;
using Assets.Scripts.ThirdParty.Events;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Linq;

namespace Assets.Scripts.Gameplay.Handlers.PlayerActions.Implementations
{
    public class PlayerActionSlide : PlayerAction
    {
        private ITile[] _tiles;

        public PlayerActionSlide(Player player, ITile[] tiles, float time = 0.15f, Ease ease = Ease.OutCubic) : base(player, time, ease)
        {
            _tiles = tiles;
        }

        public override void Execute(Action onStart = null, Action onComplete = null)
        {
            onStart?.Invoke();

            var sequence = DOTween.Sequence();
            var path = new Path(PathType.Linear, _tiles.Select(tile => tile.WorldPosition + Player.WorldOffset).ToArray(), 10);
            var time = Time * (_tiles.Length - 1);

            sequence
                .Append(Player.Transform.DOPath(path, time)
                .OnWaypointChange(value =>
                {
                    if (value != _tiles.Length - 1)
                    {
                        _tiles[value].OnPlayerExit(Player);
                    }
                })
                .SetEase(Ease))
                .OnComplete(() =>
                {
                    var toTile = _tiles[_tiles.Length - 1];
                    Player.SetPosition(toTile);
                    onComplete?.Invoke();
                    toTile.OnPlayerEnter(Player);

                    EventManager.TriggerEvent(new PlayerActionCompleteEvent(Player));
                });
        }

        public override bool CanExecute()
        {
            return true;
        }
    }
}   