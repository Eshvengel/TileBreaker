﻿using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI.Panels.Implementations
{
    public class LevelsPanel : Panel
    {
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            startPosition = rectTransform.localPosition;
            endPosition = rectTransform.localPosition + Vector3.right * rectTransform.rect.size.x;

            rectTransform.localPosition = endPosition;

            closeButton.AddListener(Hide);
        }

        public override void Show()
        {
            if (inProcess || rectTransform.localPosition == startPosition) return;

            rectTransform
                .DOLocalMove(startPosition, TimeOut)
                .SetEase(easeOut)
                .OnStart(() =>
                {
                    inProcess = true;
                })
                .OnComplete(() =>
                {
                    inProcess = false;
                });
        }

        public override void Hide()
        {
            if (inProcess || rectTransform.localPosition == endPosition) return;

            rectTransform
                .DOLocalMove(endPosition, TimeIn)
                .SetEase(easeIn)
                .OnStart(() =>
                {
                    inProcess = true;
                })
                .OnComplete(() =>
                {
                    inProcess = false;
                });
        }
    }
}
