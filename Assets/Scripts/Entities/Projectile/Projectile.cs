using Zenject;
using Services;
using UnityEngine;
using IPoolable = Services.IPoolable;

namespace Entities
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        [Inject] private IPoolService _poolService;

        public GameObject GameObject => gameObject;

        public void OnSpawn()
        {
        }

        public void OnDespawn()
        {
        }
        
        public void Hit()
        {
            _poolService.Despawn(this);
        }
    }
}