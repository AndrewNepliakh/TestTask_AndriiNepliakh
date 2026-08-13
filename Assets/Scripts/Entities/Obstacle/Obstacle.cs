using Services;
using UnityEngine;

namespace Entities.Obstacle
{
    public class Obstacle : MonoBehaviour, IPoolable
    {
        public GameObject GameObject => gameObject;
        
        public void OnSpawn()
        {
        }

        public void OnDespawn()
        {
        }
    }
}