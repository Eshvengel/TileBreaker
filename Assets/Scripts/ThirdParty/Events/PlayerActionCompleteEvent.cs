using Assets.Scripts.Gameplay;

namespace Assets.Scripts.ThirdParty.Events
{
    public class PlayerActionCompleteEvent : Event
    {
        public Player Player { get; }

        public PlayerActionCompleteEvent(Player player)
        {
            Player = player;
        }
    }
}