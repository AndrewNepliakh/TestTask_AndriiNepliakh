using Zenject;
using Services;
using UnityEngine;

namespace Entities
{
    public class GameplaySphereShootAttribute : MonoBehaviour
    {
        [Inject] private IPoolService _poolService;

        [SerializeField] private Transform _projectileSpawnPoint;

        public void Shoot()
        {
            _poolService.Spawn<Projectile>(
                _projectileSpawnPoint.position,
                Quaternion.identity);
        }
    }
}