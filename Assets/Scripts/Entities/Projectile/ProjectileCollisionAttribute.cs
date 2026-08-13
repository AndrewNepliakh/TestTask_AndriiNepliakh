using System;
using Zenject;
using Services;
using UnityEngine;

namespace Entities
{
    public class ProjectileCollisionAttribute : MonoBehaviour
    {
        [Inject] private IPoolService _poolService;

        [SerializeField] private Projectile _projectile;
        [SerializeField] private ProjectileHitAttribute _hitAttribute;

        public event Action OnCollision;

        private void OnTriggerEnter(Collider other)
        {
            _hitAttribute.Hit(OnHitCompleted);
        }

        private void OnHitCompleted()
        {
            OnCollision?.Invoke();

            _poolService.Despawn(_projectile);
        }
    }
}