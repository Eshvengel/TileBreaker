using Assets.Scripts.Data.Levels;

namespace Assets.Scripts.ThirdParty.Events
{
    public class StartBuildLevelEvent : Event
    {
        public int LevelId { get; }

        public StartBuildLevelEvent(int levelId)
        {
            LevelId = levelId;
        }
    }
}