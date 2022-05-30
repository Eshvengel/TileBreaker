using Assets.Scripts.Gameplay.Field;

namespace Assets.Scripts.ThirdParty.Events
{
    public class GamePlayStartEvent : Event
    {
        public GameField Field { get; }

        public GamePlayStartEvent(GameField field)
        {
            Field = field;
        }
    }
}