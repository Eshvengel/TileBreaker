using Assets.Scripts.Data.TilesData;

namespace Assets.Scripts.Gameplay.Tiles.Implementations
{
    public class TileCommon : Tile<TileCommonData>
    {
        public override void OnPlayerEnter(Player player)
        {

        }

        public override void OnPlayerExit(Player player)
        {
            Hide();
        }
    }
}
