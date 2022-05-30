using UnityEngine;

namespace Assets.Scripts.Core.RuntimeEditor.Grid
{
    public class EditorGrid : IGrid
    {
        public void Draw(float gridSize, int tileSize)
        {
            for (float x = 0; x <= gridSize; x++)
            {
                Debug.DrawLine(new Vector3(x * tileSize, 0, 0), new Vector3(x * tileSize, 0, gridSize * tileSize), Color.white, Time.deltaTime);
            }

            for (float z = 0; z <= gridSize; z++)
            {
                Debug.DrawLine(new Vector3(0, 0, z * tileSize), new Vector3(gridSize * tileSize, 0, z * tileSize), Color.white, Time.deltaTime);
            }
        }
    }
}
