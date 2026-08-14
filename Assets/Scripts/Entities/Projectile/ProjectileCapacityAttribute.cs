using UnityEngine;

namespace Entities
{
    public class ProjectileCapacityAttribute : MonoBehaviour
    {
        [SerializeField] private Transform _scaleTransform;
        [SerializeField] private float _radiusMultiplier = 2f;

        public float Radius { get; private set; }

        public void SetCapacity(float capacity)
        {
            _scaleTransform.localScale = (Vector3.one * capacity) * 3f;

            Radius = (capacity * _radiusMultiplier) * 1.5f;
        }
    }
}