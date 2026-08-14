using System;
using Entities;

namespace Managers
{
    public interface IGameplaySphereManager
    {
        GameplaySphere GameplaySphere { get; }

        event Action<GameplaySphere> OnGameplaySphereCreated;

        void SpawnGameplaySphere();
        void DespawnGameplaySphere();
    }
}