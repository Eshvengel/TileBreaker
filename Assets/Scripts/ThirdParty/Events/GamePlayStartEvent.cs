using Assets.Scripts.Gameplay.Field;

namespace Assets.Scripts.ThirdParty.Events
{
    public class GamePlayStartEvent : Event
    {
        public int LevelId { get; }
        public GameField Field { get; }
        

        public GamePlayStartEvent(GameField field, int levelId)
        {
            Field = field;
            LevelId = levelId;
        }
    }
}