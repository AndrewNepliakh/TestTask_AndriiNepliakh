using Zenject;
using Managers;
using Services;
using UnityEngine;
using IPoolable = Services.IPoolable;

namespace Entities
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        [Inject] private IGameManager _gameManager;
        [Inject] private IPoolService _poolService;
        
        public GameObject GameObject => gameObject;

        public void OnSpawn()
        {
            _gameManager.OnStateChange += OnStateChange;
        }

        private void OnStateChange(GameState gameState)
        {
            if(gameState != GameState.Play) _poolService.Despawn(this);
        }


        public void OnDespawn()
        {
            _gameManager.OnStateChange -= OnStateChange;
        }
    }
}