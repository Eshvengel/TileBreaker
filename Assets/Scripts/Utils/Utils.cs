using System;
using Assets.Scripts.Data.TilesData;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class Utils
    {
        public static Vector2Int ToVector2Int(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return Vector2Int.up;
                case Direction.Down: return Vector2Int.down;
                case Direction.Left: return Vector2Int.left;
                case Direction.Right: return Vector2Int.right;
            }
            return Vector2Int.zero;
        }

        public static Direction Next(this Direction value)
        {
            var values = (Direction[])Enum.GetValues(value.GetType());
            var j = Array.IndexOf(values, value) + 1;
            return values.Length == j ? values[1] : values[j];
        }
    }
}
