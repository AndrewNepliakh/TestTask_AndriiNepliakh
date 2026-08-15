using UI;
using Zenject;
using Managers;
using Entities;
using Services;
using UnityEngine;
using System.Threading.Tasks;

namespace Controllers
{
    public class GameplayController : MonoBehaviour
    {
        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
        [Inject] private IAssetsManager _assetsManager;
        [Inject] private IObstaclesManager _obstaclesManager;

        [SerializeField] private Obstacle[] _preallocatedObstacles = new Obstacle[600];

        [Inject]
        private async void Initiate(
            InitialGameplayState initialGameplayState,
            WinGameplayState winGameplayState,
            LoseGameplayState loseGameplayState)
        {
            _gameplayStateMachine.AddState(initialGameplayState);
            _gameplayStateMachine.AddState(winGameplayState);
            _gameplayStateMachine.AddState(loseGameplayState);

            await PreloadAssets();
            
            _obstaclesManager.Initiate(_preallocatedObstacles);

            _gameplayStateMachine.ChangeState(GameplayStates.Initial);
        }

        private async Task PreloadAssets()
        {
            await _assetsManager.PreloadAssetAsync<TestHUD>();
            await _assetsManager.PreloadAssetAsync<WinPopup>();
            await _assetsManager.PreloadAssetAsync<LosePopup>();
            await _assetsManager.PreloadAssetAsync<Obstacle>();
            await _assetsManager.PreloadAssetAsync<Projectile>();
            await _assetsManager.PreloadAssetAsync<GameplaySphere>();
        }
    }
}