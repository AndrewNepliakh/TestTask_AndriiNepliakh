using Zenject;
using Services;
using UnityEngine;

namespace Entities
{
    public class GameplaySphereShootAttribute : MonoBehaviour
    {
        [Inject] private IPoolService _poolService;

        [SerializeField] private Transform _projectileSpawnPoint;
        [SerializeField] private GameplaySphereTapAttribute _tapAttribute;
        [SerializeField] private GameplaySphereCapacityAttribute _capacityAttribute;
        [SerializeField] private GameplaySphereCheckDoorAttribute _checkDoorAttribute;
        
        private ProjectileHitAttribute _projectileHit;

        private bool _canShoot = true;

        private void OnEnable()
        {
            _capacityAttribute.OnCapacitySpent += Shoot;
        }

        private void OnDisable()
        {
            _capacityAttribute.OnCapacitySpent -= Shoot;

            if (_tapAttribute != null)
                _tapAttribute.SetCanReceiveTap(true);

            _canShoot = true;
        }

        private void Shoot(float spentCapacity)
        {
            if (!_canShoot)
                return;

            if (spentCapacity <= 0f)
                return;

            _canShoot = false;

            _tapAttribute.SetCanReceiveTap(false);

            var projectile = _poolService.Spawn<Projectile>(
                _projectileSpawnPoint.position,
                Quaternion.identity);

            var projectileCapacity =
                projectile.GetComponent<ProjectileCapacityAttribute>();

            projectileCapacity.SetCapacity(spentCapacity);

            _projectileHit =
                projectile.GetComponentInChildren<ProjectileHitAttribute>();

            _projectileHit.OnAfterObstaclesDestroyed += OnProjectileHit;
            
            _checkDoorAttribute.RegisterProjectile(_projectileHit);
        }
        
        private void OnProjectileHit()
        {
            if (_projectileHit != null)
            {
                _projectileHit.OnAfterObstaclesDestroyed -= OnProjectileHit;
                _projectileHit = null;
            }

            _tapAttribute.SetCanReceiveTap(true);

            _canShoot = true;
        }
    }
}