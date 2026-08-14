using System;
using Zenject;
using Services;
using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

namespace Entities
{
    public class ProjectileHitAttribute : MonoBehaviour
    {
        [Inject] private IPoolService _poolService;

        [SerializeField] private ProjectileCapacityAttribute _capacityAttribute;
        [SerializeField] private float _delay = 0.5f;

        private int _obstacleLayer;
        private int _wallLayer;

        public event Action OnAfterObstaclesDestroyed;

        private void Awake()
        {
            _obstacleLayer = LayerMask.NameToLayer("Obstacle");
            _wallLayer = LayerMask.NameToLayer("Wall");
        }

        public void Hit(Action onComplete)
        {
            var radius = _capacityAttribute.Radius;

            var colliders = Physics.OverlapSphere(
                transform.position,
                radius,
                1 << _obstacleLayer);

            var obstacles = new HashSet<Obstacle>();

            foreach (var collider in colliders)
            {
                var obstacle = collider.GetComponentInParent<Obstacle>();

                if (obstacle != null)
                    obstacles.Add(obstacle);
            }

            foreach (var obstacle in obstacles)
            {
                obstacle
                    .GetComponent<ObstacleHitAttribute>()
                    ?.SetHit();
            }

            _poolService.Despawn(GetComponent<Projectile>());
            
            DOVirtual.DelayedCall(_delay, () =>
            {
                foreach (var obstacle in obstacles)
                {
                    if (obstacle != null && obstacle.GameObject.activeSelf)
                        _poolService.Despawn(obstacle);
                }

                onComplete?.Invoke();
                
                OnAfterObstaclesDestroyed?.Invoke();
            });
        }

        private void OnDrawGizmosSelected()
        {
            if (_capacityAttribute == null)
                return;

            Gizmos.DrawWireSphere(
                transform.position,
                _capacityAttribute.Radius);
        }
    }
}