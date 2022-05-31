using UnityEngine;

namespace Assets.Scripts.Gameplay.Objects.Collectable
{
    public class Coin : CollectableObject
    {
        private float _currentAngle = 0;
        private float _angleStep = 1;

        protected override void Move()
        {
            if (_currentAngle >= 360)
                _currentAngle = 0;

            transform.rotation = Quaternion.AngleAxis(_currentAngle, Vector3.up);

            _currentAngle += _angleStep;
        }
    }
}
