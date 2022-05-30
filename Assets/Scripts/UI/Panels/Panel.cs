using Assets.Scripts.UI.Buttons;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI.Panels
{
    public abstract class Panel : MonoBehaviour, IPanel
    {
        [SerializeField] 
        protected Button closeButton;
        
        [SerializeField] 
        protected Ease easeIn;
        
        [SerializeField] 
        protected Ease easeOut;
        
        [SerializeField] 
        protected float TimeIn = 0.2f;
        
        [SerializeField] 
        protected float TimeOut = 0.5f;
        
        protected RectTransform rectTransform;
        protected bool inProcess;
        protected Vector3 startPosition;
        protected Vector3 endPosition;

        public abstract void Show();
        public abstract void Hide();
    }
}
