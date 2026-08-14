using Entities;

namespace Managers
{
    public interface IGameplaySphereManager
    {
        GameplaySphere GameplaySphere { get; }

        void SpawnGameplaySphere();
        void DespawnGameplaySphere();
    }
}