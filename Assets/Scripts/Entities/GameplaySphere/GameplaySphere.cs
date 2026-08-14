using Services;
using UnityEngine;
using Sirenix.Utilities;


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

        public void Initiate()
        {
            GetComponentsInChildren<IInitializer>().ForEach(x => x.Initialize());
        }
    }
}