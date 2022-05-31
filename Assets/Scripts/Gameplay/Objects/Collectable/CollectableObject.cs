using UnityEngine;

namespace Assets.Scripts.Gameplay.Objects.Collectable
{
    public abstract class CollectableObject : MonoBehaviour, ICollectable
    {
        protected abstract void Move();
        
        private void Update()
        {
            Move();
        }
        
        public void Collect()
        {
            // Do some things.
        }
    }
}