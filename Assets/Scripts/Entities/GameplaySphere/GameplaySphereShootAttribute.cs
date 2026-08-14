using Zenject;
using Services;
using UnityEngine;

namespace Entities
{
    public class GameplaySphereShootAttribute : MonoBehaviour
    {
        [Inject] private IPoolService _poolService;

        [SerializeField] private GameplaySphereTapAttribute _tapAttribute;
        [SerializeField] private Transform _projectileSpawnPoint;

        private ProjectileCollisionAttribute _projectileCollision;
        private bool _canShoot = true;

        private void OnEnable()
        {
            _tapAttribute.OnPointerUpEvent += Shoot;
        }

        private void OnDisable()
        {
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

            _canShoot = false;

            var projectile = _poolService.Spawn<Projectile>(
                _projectileSpawnPoint.position,
                Quaternion.identity);

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