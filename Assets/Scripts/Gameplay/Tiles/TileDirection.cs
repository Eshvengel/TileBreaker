using Assets.Scripts.Data.TilesData;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Tiles
{
    public class TileDirection : MonoBehaviour
    {
        private const int ROTATION_STEP = 90;
        public Transform Transform { get; private set; }
        private void Awake()
        {
            Transform = transform;
        }
        
        public void SetDirection(Direction tileDirection)
        {
            Transform.rotation = GetRotationByTileDirection(tileDirection);
        }

        private Quaternion GetRotationByTileDirection(Direction tileDirection)
        {
            switch (tileDirection)
            {
                case Direction.Up: return Quaternion.Euler(0, 180, 0);
                case Direction.Down: return Quaternion.Euler(0, 0, 0);
                case Direction.Left: return Quaternion.Euler(0, 90, 0);
                case Direction.Right: return Quaternion.Euler(0, 270, 0);
            }
            
            return Quaternion.identity;
        }
    }
}