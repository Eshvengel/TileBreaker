using UnityEngine;

namespace Assets.Scripts.Data.TilesData
{
    public class TileJumperData : TileData
    {
        public TileJumperData(TileType type, int x, int y, Vector3 worldPosition, Direction jumpDirection, int jumpPower)
        {
            Type = type;
            X = x;
            Y = y;
            WorldPosition = worldPosition;
            JumpDirection = jumpDirection;
            JumpPower = jumpPower;
        }
    }
}
