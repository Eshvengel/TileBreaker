using System;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Implementations
{
    public class Screenloader : MonoBehaviour
    {
        private static Screenloader instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }


        public static void Show(Action callback)
        {
            instance.RealShow(callback);
        }

        public static void Hide(Action callback)
        {
            instance.RealHide(callback);
        }

        private void RealShow(Action callback)
        {
            // Open screenloader

            // Call callback
            callback?.Invoke();
        }

        private void RealHide(Action callback)
        {
            // Open screenloader

            // Call callback
            callback?.Invoke();
        }


    }
}
