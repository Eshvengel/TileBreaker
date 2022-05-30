using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI.Panels.Implementations
{
    public class SettingsPanel : Panel
    {
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            startPosition = rectTransform.localPosition;
            endPosition = rectTransform.localPosition + Vector3.down * rectTransform.rect.size.y;

            rectTransform.localPosition = endPosition;

            closeButton.AddListener(Hide);
        }

        public override void Show()
        {
            if (inProcess || transform.localPosition == startPosition) return;

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
            if (inProcess || transform.localPosition == endPosition) return;

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
