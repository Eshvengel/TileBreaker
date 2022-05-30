using UnityEngine;

namespace Assets.Scripts.Data.TilesData
{
    public class TileStartData : TileData
    {
        public TileStartData(TileType type, int x, int y, Vector3 worldPosition)
        {
            Type = type;
            X = x;
            Y = y;
            WorldPosition = worldPosition;
        }
    }
}
