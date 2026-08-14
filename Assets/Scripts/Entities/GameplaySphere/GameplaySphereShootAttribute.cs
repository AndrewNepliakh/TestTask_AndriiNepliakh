using Zenject;
using Services;
using UnityEngine;

namespace Entities
{
    public class GameplaySphereShootAttribute : MonoBehaviour
    {
        [Inject] private IPoolService _poolService;

        [SerializeField] private GameplaySphereCapacityAttribute _capacityAttribute;
        [SerializeField] private GameplaySphereCheckDoorAttribute _checkDoorAttribute;
        [SerializeField] private Transform _projectileSpawnPoint;

        private ProjectileCollisionAttribute _projectileCollision;
        private ProjectileHitAttribute _projectileHit;

        private bool _canShoot = true;

        private void OnEnable()
        {
            _capacityAttribute.OnCapacitySpent += Shoot;
        }

        private void OnDisable()
        {
            _capacityAttribute.OnCapacitySpent -= Shoot;

            if (_projectileCollision != null)
            {
                _projectileCollision.OnCollision -= OnProjectileCollision;
                _projectileCollision = null;
            }

            _canShoot = true;
        }

        private void Shoot(float spentCapacity)
        {
            if (!_canShoot)
                return;

            if (spentCapacity <= 0f)
                return;

            _canShoot = false;

            var projectile = _poolService.Spawn<Projectile>(
                _projectileSpawnPoint.position,
                Quaternion.identity);

            var projectileCapacity =
                projectile.GetComponent<ProjectileCapacityAttribute>();

            projectileCapacity.SetCapacity(spentCapacity);

            _projectileCollision =
                projectile.GetComponentInChildren<ProjectileCollisionAttribute>();

            _projectileCollision.OnCollision += OnProjectileCollision;

            _projectileHit =
                projectile.GetComponentInChildren<ProjectileHitAttribute>();

            _checkDoorAttribute.RegisterProjectile(_projectileHit);
        }

        private void OnProjectileCollision()
        {
            if (_projectileCollision != null)
            {
                _projectileCollision.OnCollision -= OnProjectileCollision;
                _projectileCollision = null;
            }

            _canShoot = true;
        }
    }
}