using Services;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class ProjectileHitAttribute : MonoBehaviour
    {
        [Inject] private IPoolService _poolService;

        [SerializeField] private float _radius = 3f;

        private int _obstacleLayer;

        private void Awake()
        {
            _obstacleLayer = LayerMask.NameToLayer("Obstacle");
        }

        public void Hit()
        {
            var colliders = Physics.OverlapSphere(
                transform.position,
                _radius);

            foreach (var collider in colliders)
            {
                var obstacle = collider.GetComponentInParent<Obstacle>();

                if (obstacle == null)
                    continue;

                if (obstacle.gameObject.layer != _obstacleLayer)
                    continue;

                _poolService.Despawn(obstacle);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}