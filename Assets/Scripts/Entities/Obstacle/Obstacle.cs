using Services;
using UnityEngine;
using Sirenix.Utilities;

namespace Entities
{
    public class Obstacle : MonoBehaviour, IPoolable
    {
        public GameObject GameObject => gameObject;
        
        public void Initiate()
        {
            GetComponentsInChildren<IInitializer>().ForEach(x => x.Initialize());
        }
        
        public void OnSpawn()
        {
        }

        public void OnDespawn()
        {
        }
    }
}