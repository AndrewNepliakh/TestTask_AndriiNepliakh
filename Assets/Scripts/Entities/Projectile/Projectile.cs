using Services;
using UnityEngine;

namespace Entities
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private float _speed = 10f;

        public GameObject GameObject => gameObject;

        public void OnSpawn()
        {
        }

        public void OnDespawn()
        {
        }

        private void Update()
        {
            transform.position += Vector3.back * (_speed * Time.deltaTime);
        }
    }
}