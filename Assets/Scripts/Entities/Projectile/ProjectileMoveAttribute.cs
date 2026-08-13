using UnityEngine;

namespace Entities
{
    public class ProjectileMoveAttribute : MonoBehaviour
    {
        [SerializeField] private Projectile _projectile;
        [SerializeField] private float _speed = 10f;

        private void Update()
        {
            _projectile.transform.position +=
                Vector3.back * (_speed * Time.deltaTime);
        }
    }
}