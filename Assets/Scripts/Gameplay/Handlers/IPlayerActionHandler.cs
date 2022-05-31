using Assets.Scripts.Gameplay.Handlers.PlayerActions;

namespace Assets.Scripts.Gameplay.Handlers
{
    public interface IPlayerActionHandler
    {
        bool InProcess { get; }
        void MakeAction(IPlayerAction playerAction);

        void MakeActionNow(IPlayerAction playerAction);

        void Dispose();
    }
}