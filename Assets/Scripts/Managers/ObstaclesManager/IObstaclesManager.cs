namespace Managers
{
    public interface IObstaclesManager
    {
        void SpawnObstacles(LevelConfigData levelConfig);

        void Clear();
    }
}