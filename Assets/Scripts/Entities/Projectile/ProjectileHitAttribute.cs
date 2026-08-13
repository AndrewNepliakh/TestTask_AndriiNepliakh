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

        [SerializeField] private float _radius = 3f;
        [SerializeField] private float _delay = 0.5f;

        private int _obstacleLayer;

        private void Awake()
        {
            _obstacleLayer = LayerMask.NameToLayer("Obstacle");
        }

        public void Hit(Action onComplete)
        {
            var colliders = Physics.OverlapSphere(
                transform.position,
                _radius,
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
            });
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}