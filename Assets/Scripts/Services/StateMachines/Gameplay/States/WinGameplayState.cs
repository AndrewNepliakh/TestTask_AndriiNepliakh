using UI;
using System;
using Zenject;
using UnityEngine;
using System.Threading.Tasks;

namespace Services
{
    public class WinGameplayState : IState<GameplayStates>
    {
        [Inject] private IUIManager _uiManager;
        
        public GameplayStates State => GameplayStates.Win;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;

        private WinPopup _winPopup;
        
        public async Task Enter(ChangeStateData changeStateData = null)
        {
            try
            {
                _winPopup = await _uiManager.ShowPopup<WinPopup>();

                _winPopup.OnContinueButtonClicked += OnContinueButtonClicked;
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
            _winPopup.OnContinueButtonClicked -= OnContinueButtonClicked;
            
            _uiManager.HideCurrentPopup();

            _winPopup = null;
        }
    }
}