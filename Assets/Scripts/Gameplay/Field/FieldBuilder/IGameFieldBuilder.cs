using Assets.Scripts.Data.Levels;

namespace Assets.Scripts.Gameplay.Field.FieldBuilder
{
    public interface IGameFieldBuilder
    {
        GameField GameField { get; }
        void Build(Level level);
        void Rebuild();
    }
}
