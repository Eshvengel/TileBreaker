using Assets.Scripts.UI.Buttons;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    public abstract class Window : MonoBehaviour
    {
        private const float OPEN_TIME = 0.5f;
        private const float CLOSE_TIME = 0.25f;

        public WindowState State { get; private set; }

        [SerializeField]
        private GameObject _container;
        
        [SerializeField]
        private GameObject _content;
        
        [SerializeField] 
        private Button _closeButton;

        private void Awake()
        {
            if (_closeButton != null) 
                _closeButton.AddListener(Hide);

            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        protected virtual void AddListeners() { }
        protected virtual void RemoveListeners() { }

        public void Open()
        {
            Show();
        }

        public virtual void Show()
        {
            #region Validate

            if (State == WindowState.Process || State == WindowState.Open)
            {
                Debug.LogWarning($"Window {gameObject.name} is already open!");
                return;
            }

            #endregion

            OnShow();

            State = WindowState.Process;

            _content.transform
                .DOScale(Vector3.one, OPEN_TIME)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    State = WindowState.Open;
                });
        }

        public virtual void Hide()
        {
            #region Validate

            if (State == WindowState.Process || State == WindowState.Hide)
            {
                Debug.LogWarning($"Window {gameObject.name} is already hide!");
                return;
            }

            #endregion

            State = WindowState.Process;

            _content.transform
                .DOScale(Vector3.zero, CLOSE_TIME)
                .SetEase(Ease.InSine)
                .OnComplete(() =>
                {
                    OnHide();

                    State = WindowState.Hide;
                });
        }

        private void OnShow()
        {
            _content.transform.localScale = Vector3.zero;
            transform.SetAsLastSibling();
            _container.SetActive(true);
        }


        private void OnHide()
        {
            _content.transform.localScale = Vector3.zero;
            _container.SetActive(false);
        }
    }
}
