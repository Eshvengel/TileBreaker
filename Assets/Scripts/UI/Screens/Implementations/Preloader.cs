using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Screens.Implementations
{
    public class Preloader : MonoBehaviour
    {
        private const float PROGRESS_TIME_STEP = 0.0001f;

        public static Preloader Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                
                instance = FindObjectOfType<Preloader>();
                
                return instance;
            }
        }

        [SerializeField] 
        private GameObject _container;
        
        [SerializeField] 
        private Slider _progressSlider;

        private static Preloader instance;
        private int _curProgress;
        private int _realProgress;

        private void Awake()
        {
            StartCoroutine(Loading());
        }

        private IEnumerator Loading()
        {
            while (_curProgress < 99)
            {
                if (_curProgress < _realProgress)
                {
                    _progressSlider.value = _curProgress / 100f;
                    _curProgress++;
                }

                yield return new WaitForSeconds(PROGRESS_TIME_STEP);
            }

            Hide();
        }

        public static void OnProgress(int value)
        {
            Instance._realProgress = value;
        }

        private void Hide()
        {
            _container.SetActive(false);
        }
    }
}
