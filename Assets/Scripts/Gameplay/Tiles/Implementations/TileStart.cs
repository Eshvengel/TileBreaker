using Assets.Scripts.Data.TilesData;

namespace Assets.Scripts.Gameplay.Tiles.Implementations
{
    public class TileStart : Tile<TileStartData>
    {
        public override void OnPlayerEnter(Player player)
        {
            // Do nothing
        }

        public override void OnPlayerExit(Player player)
        {
            Hide();
        }
    }
}
