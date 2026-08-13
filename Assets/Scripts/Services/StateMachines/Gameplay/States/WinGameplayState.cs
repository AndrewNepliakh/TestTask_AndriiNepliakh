using System;
using Zenject;
using UnityEngine;
using System.Threading.Tasks;

namespace Services
{
    public class WinGameplayState : IState<GameplayStates>
    {
        public GameplayStates State => GameplayStates.Win;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
        
        public Task Enter(ChangeStateData changeStateData = null)
        {
            try
            {
                return Task.CompletedTask;
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