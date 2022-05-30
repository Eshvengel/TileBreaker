using Assets.Scripts.Data.Levels;

namespace Assets.Scripts.ThirdParty.Events
{
    public class StartBuildLevelEvent : Event
    {
        public Level Level { get; }

        public StartBuildLevelEvent(Level level)
        {
            Level = level;
        }
    }
}