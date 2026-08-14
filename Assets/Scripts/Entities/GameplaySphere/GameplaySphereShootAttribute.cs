using Zenject;
using Services;
using UnityEngine;

namespace Entities
{
    public class GameplaySphereShootAttribute : MonoBehaviour
    {
        [Inject] private IPoolService _poolService;

        [SerializeField] private GameplaySphereTapAttribute _tapAttribute;
        [SerializeField] private GameplaySphereCapacityAttribute _capacityAttribute;
        [SerializeField] private Transform _projectileSpawnPoint;

        private ProjectileCollisionAttribute _projectileCollision;
        private bool _canShoot = true;

        private void OnEnable()
        {
            _tapAttribute.OnPointerUpEvent += Shoot;
        }

        private void OnDisable()
        {
            if (_tapAttribute != null)
                _tapAttribute.OnPointerUpEvent -= Shoot;

            if (_projectileCollision != null)
            {
                _projectileCollision.OnCollision -= OnProjectileCollision;
                _projectileCollision = null;
            }

            _canShoot = true;
        }

        private void Shoot()
        {
            if (!_canShoot)
                return;

            var spentCapacity = _capacityAttribute.SpentCapacity;

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
        }

        private void OnProjectileCollision()
        {
            if (_projectileCollision == null)
                return;

            _projectileCollision.OnCollision -= OnProjectileCollision;
            _projectileCollision = null;

            _canShoot = true;
        }
    }
}