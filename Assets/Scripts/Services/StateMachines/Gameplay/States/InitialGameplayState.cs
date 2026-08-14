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
        [Inject] private IGameManager _gameManager;
        [Inject] private ILevelManager _levelManager;
        [Inject] private IObstaclesManager _obstaclesManager;
        [Inject] private IGameplaySphereManager _gameplaySphereManager;
        
        public GameplayStates State => GameplayStates.Initial;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;

        public async Task Enter(ChangeStateData changeStateData)
        {
            _gameManager.OnStateChange += OnStateChange;
            
            _gameManager.OnPlay();
            
            try
            {
                await _uiManager.ShowHUDWindow<TestHUD>();

                var currentLevelData = _levelManager.GetLevelConfigOfCurrentLevel();
                
                _obstaclesManager.SpawnObstacles(currentLevelData);
                
                _gameplaySphereManager.SpawnGameplaySphere();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        private void OnStateChange(GameState gameState)
        {
            if (gameState == GameState.Win)
            {
                _gameplayStateMachine.ChangeState(GameplayStates.Win);
            }
            
            if (gameState == GameState.Lose)
            {
                _gameplaySphereManager.DespawnGameplaySphere();
                
                _gameplayStateMachine.ChangeState(GameplayStates.Lose);
            }
        }

        public void Exit()
        {
            _uiManager.HideHUDWindow();
            
            _gameManager.OnStateChange -= OnStateChange;
        }
    }
}