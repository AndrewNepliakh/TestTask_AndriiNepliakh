using Services;
using UnityEngine;

namespace Entities.GameplaySphere
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