using UnityEngine;

namespace Assets.Scripts.Core.RuntimeEditor.Brush
{
    public class EditorTileBrush : IBrush
    {
        public int LogicPositionX => _logicPositionX;
        public int LogicPositionY => _logicPositionY;

        private int _logicPositionX;
        private int _logicPositionY;
        private readonly Camera _camera;

        public EditorTileBrush(Camera camera)
        {
            _camera = camera;
        }

        public void Draw(float gridSize, int tileSize)
        {
            UpdateBrushLogicPosition(tileSize);

            if (IsBrushOverGrid(gridSize)) return;

            //Right
            Debug.DrawLine(new Vector3(_logicPositionX + tileSize, 0, _logicPositionY + tileSize), new Vector3(_logicPositionX + tileSize, 0, _logicPositionY), Color.red, Time.deltaTime);

            // Up
            Debug.DrawLine(new Vector3(_logicPositionX, 0, _logicPositionY + tileSize), new Vector3(_logicPositionX + tileSize, 0, _logicPositionY + tileSize), Color.red, Time.deltaTime);

            //Left
            Debug.DrawLine(new Vector3(_logicPositionX, 0, _logicPositionY), new Vector3(_logicPositionX, 0, _logicPositionY + tileSize), Color.red, Time.deltaTime);

            //Down
            Debug.DrawLine(new Vector3(_logicPositionX, 0, _logicPositionY), new Vector3(_logicPositionX + tileSize, 0, _logicPositionY), Color.red, Time.deltaTime);
        }

        public bool IsBrushOverGrid(float gridSize)
        {
            return _logicPositionX < 0 || _logicPositionX >= gridSize || _logicPositionY >= gridSize || _logicPositionY < 0;
        }

        private void UpdateBrushLogicPosition(int tileSize)
        {
            var worldPosition = GetBrushWorldPosition();

            _logicPositionX = Mathf.FloorToInt(worldPosition.x / tileSize);
            _logicPositionY = Mathf.FloorToInt(worldPosition.y / tileSize);
        }

        private Vector2 GetBrushWorldPosition()
        {
            Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            return new Vector2(mouseWorldPosition.x, mouseWorldPosition.z);
        }
    }
}
