using UnityEngine;

namespace Assets.Scripts.UI.Screens
{
    public abstract class Screen : MonoBehaviour
    {
        protected void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
