using UnityEngine;

namespace Assets.Scripts.Data.TilesData
{
    public class TileSlideData : TileData
    {
        public TileSlideData(TileType type, int x, int y, Vector3 worldPosition, Direction slideDirection, int slidePower)
        {
            Type = type;
            X = x;
            Y = y;
            WorldPosition = worldPosition;
            SlideDirection = slideDirection;
            SlidePower = slidePower;
        }
    }
}
