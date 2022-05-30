using System;
using Assets.Scripts.Data.TilesData;

namespace Assets.Scripts.Gameplay.Tiles.Implementations
{
    public class TileSolid : Tile<TileSolidData>
    {
        public override void OnPlayerEnter(Player player)
        {
            // Do nothing;
        }

        public override void OnPlayerExit(Player player)
        {
            // Do nothing;
        }
    }
}
