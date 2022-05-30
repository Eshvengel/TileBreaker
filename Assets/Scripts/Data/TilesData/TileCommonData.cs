using UnityEngine;

namespace Assets.Scripts.Data.TilesData
{
    public class TileCommonData : TileData
    {
        public TileCommonData(TileType type, int x, int y, Vector3 worldPosition)
        {
            Type = type;
            X = x;
            Y = y;
            WorldPosition = worldPosition;
        }
    }
}
