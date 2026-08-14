using Zenject;
using Services;
using Entities;
using UnityEngine;

namespace Managers
{
    public class GameplaySphereManager : IGameplaySphereManager
    {
        [Inject] private IPoolService _poolService;

        public GameplaySphere GameplaySphere { get; private set; }

        public void SpawnGameplaySphere()
        {
            if (GameplaySphere != null)
                return;

            GameplaySphere = _poolService.Spawn<GameplaySphere>(Vector3.zero, Quaternion.identity);
        }

        public void DespawnGameplaySphere()
        {
            if (GameplaySphere == null)
                return;

            _poolService.Despawn(GameplaySphere);

            GameplaySphere = null;
        }
    }
}