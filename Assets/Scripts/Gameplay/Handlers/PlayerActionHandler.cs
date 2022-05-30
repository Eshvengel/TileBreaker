using Assets.Scripts.Gameplay.Handlers.PlayerActions;

namespace Assets.Scripts.Gameplay.Handlers
{
    public class PlayerActionHandler : IPlayerActionHandler
    {
        public bool InProcess { get; private set; }

        private IPlayerAction _lastPlayerAction; 

        public void MakeAction(IPlayerAction playerAction)
        {
            if (!InProcess && playerAction.CanExecute())
            {
                playerAction.Execute(Lock, Unlock);

                _lastPlayerAction = playerAction;
            }
        }
        private void Lock()
        {
            InProcess = true;
        }
        
        private void Unlock()
        {
            InProcess = false;
        }
    }
}
