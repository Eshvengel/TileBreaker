using System;

namespace Assets.Scripts.Gameplay.Handlers.PlayerActions
{
    public interface IPlayerAction
    {
        void Execute(Action onStart = null, Action onComplete = null);
        bool CanExecute();
    }
}
