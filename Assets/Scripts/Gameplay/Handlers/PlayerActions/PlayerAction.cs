using System;
using Assets.Scripts.Gameplay.Field;
using DG.Tweening;

namespace Assets.Scripts.Gameplay.Handlers.PlayerActions
{
    public abstract class PlayerAction : IPlayerAction
    {
        public Player Player { get; }
        public float Time { get; }
        public Ease Ease { get; }

        protected PlayerAction(Player player, float time, Ease ease)
        {
            Player = player;
            Time = time;
            Ease = ease;
        }

        public abstract void Execute(Action onStart = null, Action onComplete = null);
        public abstract bool CanExecute();
    }
}