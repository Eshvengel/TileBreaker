using Assets.Scripts.Data.TilesData;
using Assets.Scripts.Gameplay.Handlers.PlayerActions.Implementations;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Tiles.Implementations
{
    public class TileJump : Tile<TileJumperData>
    {
        public override void OnPlayerEnter(Player player)
        {
            Vector2Int direction = Data.JumpDirection.ToVector2Int();
            ITile tile = GameField[X + direction.x * Data.JumpPower, Y + direction.y * Data.JumpPower];

            if (tile != null && tile.Interactable)
                player.TryMakeAction(new PlayerActionJump(player, this, tile));
        }

        public override void OnPlayerExit(Player player)
        {
            Hide();
        }
    }
}
