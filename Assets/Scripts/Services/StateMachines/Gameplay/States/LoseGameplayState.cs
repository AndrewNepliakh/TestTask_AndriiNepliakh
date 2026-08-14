using UI;
using System;
using Zenject;
using Managers;
using UnityEngine;
using System.Threading.Tasks;

namespace Services
{
    public class LoseGameplayState : IState<GameplayStates>
    {
        [Inject] private IUIManager _uiManager;
        [Inject] private IGameplaySphereManager _gameplaySphereManager;
        
        public GameplayStates State => GameplayStates.Lose;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;

        private LosePopup _losePopup;
    
        public async Task Enter(ChangeStateData changeStateData = null)
        {
            try
            {
                _losePopup = await _uiManager.ShowPopup<LosePopup>();

                _losePopup.OnContinueButtonClicked += OnContinueButtonClicked;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        private void OnContinueButtonClicked()
        {
            _gameplayStateMachine.ChangeState(GameplayStates.Initial);
        }

        public void Exit()
        {
            _gameplaySphereManager.DespawnGameplaySphere();
            
            _losePopup.OnContinueButtonClicked -= OnContinueButtonClicked;
            
            _uiManager.HideCurrentPopup();

            _losePopup = null;
        }
    }
}