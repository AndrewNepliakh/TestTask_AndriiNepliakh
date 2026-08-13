using Services;
using UnityEngine;

namespace Entities
{
    public class GameplaySphere :  MonoBehaviour, IPoolable
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