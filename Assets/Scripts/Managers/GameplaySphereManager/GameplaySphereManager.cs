using System;
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

        public event Action<GameplaySphere> OnGameplaySphereCreated;

        public void SpawnGameplaySphere()
        {
            if (GameplaySphere != null)
                return;

            GameplaySphere = _poolService.Spawn<GameplaySphere>(
                Vector3.zero,
                Quaternion.identity);

            OnGameplaySphereCreated?.Invoke(GameplaySphere);
            
            GameplaySphere.Initiate();
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