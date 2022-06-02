using UnityEngine;

namespace Assets.Scripts.UI.Screens
{
    public abstract class Screen : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        protected void SetActive(bool value)
        {
            _container?.SetActive(value);
        }
    }
}
