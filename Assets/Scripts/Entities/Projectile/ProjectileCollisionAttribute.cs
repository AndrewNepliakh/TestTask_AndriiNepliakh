using Services;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class ProjectileCollisionAttribute : MonoBehaviour
    {
        [Inject] private IPoolService _poolService;

        [SerializeField] private ProjectileHitAttribute _hitAttribute;
        [SerializeField] private Projectile _projectile;

        private void OnTriggerEnter(Collider other)
        {
            _hitAttribute.Hit();

            _poolService.Despawn(_projectile);
        }
    }
}