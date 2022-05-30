namespace Assets.Scripts.Core.RuntimeEditor.Brush
{
    public interface IBrush
    {
        int LogicPositionX { get; }
        int LogicPositionY { get; }

        void Draw(float gridSize, int tileSize);
        bool IsBrushOverGrid(float gridSize);
    }
}
