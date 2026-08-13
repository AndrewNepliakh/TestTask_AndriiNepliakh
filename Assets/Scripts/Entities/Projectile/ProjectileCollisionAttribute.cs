using UnityEngine;

namespace Entities
{
    public class ProjectileCollisionAttribute : MonoBehaviour
    {
        [SerializeField] private Projectile _projectile;

        private void OnTriggerEnter(Collider other)
        {
            _projectile.Hit();
        }
    }
}