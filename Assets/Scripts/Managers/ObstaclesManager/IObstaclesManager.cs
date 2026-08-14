using System.Collections.Generic;
using Entities;

namespace Managers
{
    public interface IObstaclesManager
    {
        void Initiate(Obstacle[] preallocatedObstacles);
        
        void SpawnObstacles(LevelConfigData levelConfig);

        void Clear();
    }
}