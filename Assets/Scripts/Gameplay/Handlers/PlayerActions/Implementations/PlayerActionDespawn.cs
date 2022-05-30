using System;
using Assets.Scripts.ThirdParty;
using Assets.Scripts.ThirdParty.Events;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Handlers.PlayerActions.Implementations
{
    public class PlayerActionDespawn : PlayerAction
    {
        public PlayerActionDespawn(Player player, float time, Ease ease) : base(player, time, ease)
        {
            
        }

        public override void Execute(Action onStart = null, Action onComplete = null)
        {
            onStart?.Invoke();

            Player.Transform
                .DOMoveY(Player.WorldPosition.y * 2, Time)
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                    
                    EventManager.TriggerEvent(new PlayerActionCompleteEvent(Player));
                });
        }

        public override bool CanExecute()
        {
            return true;
        }
    }
}