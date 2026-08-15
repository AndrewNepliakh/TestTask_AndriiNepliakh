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

        [SerializeField] private Projectile _projectile;
        [SerializeField] private ProjectileCapacityAttribute _capacityAttribute;
        
        [SerializeField] private float _delay = 0.5f;

        private int _obstacleLayer;
        private bool _hasHit;

        public event Action OnAfterObstaclesDestroyed;

        private void Awake()
        {
            _obstacleLayer = LayerMask.NameToLayer("Obstacle");
        }

        private void OnEnable()
        {
            _hasHit = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasHit)
                return;

            _hasHit = true;

            Hit();
        }

        private void Hit()
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

            if (obstacles.Count == 0)
            {
                OnAfterObstaclesDestroyed?.Invoke();

                _poolService.Despawn(_projectile);
                return;
            }

            foreach (var obstacle in obstacles)
            {
                obstacle.GetComponent<ObstacleHitAttribute>()?.Hit();
            }

            _poolService.Despawn(_projectile);

            DOVirtual.DelayedCall(_delay, () =>
            {
                foreach (var obstacle in obstacles)
                {
                    if (obstacle != null && obstacle.GameObject.activeSelf)
                        _poolService.Despawn(obstacle);
                }

                OnAfterObstaclesDestroyed?.Invoke();
            });
        }
    }
}