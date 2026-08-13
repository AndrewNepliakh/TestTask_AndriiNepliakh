using UI;
using Zenject;
using Managers;
using Services;
using UnityEngine;
using Entities.Obstacle;
using System.Threading.Tasks;

namespace Controllers
{
    public class GameplayController : MonoBehaviour
    {
        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
        [Inject] private IAssetsManager _assetsManager;

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

            _gameplayStateMachine.ChangeState(GameplayStates.Initial);
        }

        private async Task PreloadAssets()
        {
            await _assetsManager.PreloadAssetAsync<TestHUD>();
            await _assetsManager.PreloadAssetAsync<Obstacle>();
        }
    }
}