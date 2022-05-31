using UnityEngine;

namespace Assets.Scripts.Gameplay.Components
{
    public class FollowTargetComponent : MonoBehaviour
    {
        [SerializeField] 
        private Transform _target;

        [SerializeField] 
        private Vector3 _offset;

        [SerializeField]
        private float _smoothTime = 0.1f;

        [SerializeField] 
        private float _maxSpeed = 10f;

        private Vector3 _velocity;
        private Transform _transform;

        private void Awake()
        {
            _transform = gameObject.transform;
        }

        private void Update()
        {
            // FollowTarget();
        }

        private void FollowTarget()
        {
            var targetPosition = new Vector3(_target.position.x + _offset.x, _offset.y, _target.position.z + _offset.z);

            _transform.position = Vector3.SmoothDamp(
                current: transform.position,
                target: targetPosition,
                currentVelocity: ref _velocity,
                smoothTime: _smoothTime,
                maxSpeed: _maxSpeed,
                deltaTime: Time.deltaTime);
        }
    }
}
