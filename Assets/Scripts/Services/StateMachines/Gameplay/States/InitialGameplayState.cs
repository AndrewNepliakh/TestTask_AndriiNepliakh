using UI;
using System;
using Zenject;
using Managers;
using UnityEngine;
using System.Threading.Tasks;

namespace Services
{
    public class InitialGameplayState : IState<GameplayStates>
    {
        [Inject] private IUIManager _uiManager;
        [Inject] private IAssetsManager _assetsManager;
        
        public GameplayStates State => GameplayStates.Initial;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;

        public async Task Enter(ChangeStateData changeStateData)
        {
            try
            {
                await _assetsManager.PreloadAssetAsync<TestHUD>();
                
                await _uiManager.ShowHUDWindow<TestHUD>();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        public void Exit()
        {
        }
    }
}