using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Buttons
{
    public class Button : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
    {
        private const float PRESS_ANIMATION_TIME = 0.1f;
        private const float UNPRESS_ANIMATION_TIME = 0.1f;

        [SerializeField]  private Transform _scaleContainer;

        private Tweener _tweener;
        private Vector3 _startScale;
        private readonly List<Action> _actions = new List<Action>();

        private void Awake()
        {
            _scaleContainer = _scaleContainer != null ? _scaleContainer : gameObject.transform;
            _startScale = _scaleContainer.localScale;
        }

        public void AddListener(Action action, bool removeAllListeners = false)
        {
            if (removeAllListeners)
                RemoveAllListeners();

            _actions.Add(action);
        }

        public void RemoveListener(Action action)
        {
            if (_actions.Contains(action))
                _actions.Remove(action);
        }

        private void RemoveAllListeners()
        {
            _actions.Clear();
        }

        private void OnClick()
        {
            if (_actions.Count <= 0)
                return;

            for (int i = 0; i < _actions.Count; i++)
            {
                _actions[i].Invoke();
            }
        }

        private void PlayPressedAnimation()
        {
            _tweener?.Kill();
            _tweener = _scaleContainer.DOScale(_startScale * 0.9f, PRESS_ANIMATION_TIME);
        }

        private void PlayUnpressedAnimation()
        {
            _tweener?.Kill();
            _tweener = _scaleContainer.DOScale(_startScale, UNPRESS_ANIMATION_TIME);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PlayUnpressedAnimation();

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PlayPressedAnimation();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick();
        }
    }
}