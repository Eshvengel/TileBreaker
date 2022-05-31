using System.Collections.Generic;
using Assets.Scripts.Data.TilesData;
using Assets.Scripts.Gameplay.Handlers.PlayerActions.Implementations;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Tiles.Implementations
{
    public class TileSlide : Tile<TileSlideData>
    {
        [SerializeField] private TileDirection _tileDirection;
        public override void OnPlayerEnter(Player player)
        {
            List<ITile> slideTiles = new List<ITile>();
            Vector2Int direction = Data.SlideDirection.ToVector2Int();

            for (int i = 0; i <= Data.SlidePower; i++)
            {
                var tile = GameField[X + direction.x * i, Y + direction.y * i];

                if (tile == null || !tile.Interactable) 
                    break;

                slideTiles.Add(GameField[X + direction.x * i, Y + direction.y * i]);
            }

            if (slideTiles.Count < 2) 
                return;

            player.TryMakeAction(new PlayerActionSlide(player, slideTiles.ToArray()));
        }

        public override void OnPlayerExit(Player player)
        {
            Hide();
        }

        protected override void OnInitialize()
        {
            _tileDirection.SetDirection(Data.SlideDirection);   
        }
    }
}
