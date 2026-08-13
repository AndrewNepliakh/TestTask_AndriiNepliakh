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
        [Inject] private ILevelManager _levelManager;
        [Inject] private IObstaclesManager _obstaclesManager;
        
        public GameplayStates State => GameplayStates.Initial;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;

        public async Task Enter(ChangeStateData changeStateData)
        {
            try
            {
                await _uiManager.ShowHUDWindow<TestHUD>();

                var currentLevelData = _levelManager.GetLevelConfigOfCurrentLevel();
                
                _obstaclesManager.SpawnObstacles(currentLevelData);
                
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