using UnityEngine;

namespace Assets.Scripts.Gameplay.Objects.Collectable
{
    public class Coin : MonoBehaviour, ICollectable
    {
        private float angle = 0;
        private float angleStep = 1;

        public void Collect()
        {
            // Записать собранную монетку

            // Уничтожить монетку
            Destroy(gameObject);
        }

        public void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            if (angle >= 360)
                angle = 0;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

            angle += angleStep;
        }
    }
}
