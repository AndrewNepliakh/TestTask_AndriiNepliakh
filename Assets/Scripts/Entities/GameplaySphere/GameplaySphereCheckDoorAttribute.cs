using Managers;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class GameplaySphereCheckDoorAttribute : MonoBehaviour
    {
        [Inject] private IGameManager _gameManager;
        
        [SerializeField] private Transform _gameplaySphere;

        private ProjectileHitAttribute _projectileHit;

        private int _obstacleLayer;

        private void Awake()
        {
            _obstacleLayer = LayerMask.NameToLayer("Obstacle");
        }

        public void RegisterProjectile(ProjectileHitAttribute projectileHit)
        {
            if (_projectileHit != null)
            {
                _projectileHit.OnAfterObstaclesDestroyed -= CheckDoor;
            }

            _projectileHit = projectileHit;

            if (_projectileHit != null)
            {
                _projectileHit.OnAfterObstaclesDestroyed += CheckDoor;
            }
        }

        private void CheckDoor()
        {
            var sphereScale = _gameplaySphere.localScale.x;

            var halfExtents = new Vector3(
                sphereScale * 0.5f,
                sphereScale * 0.5f,
                0.01f);

            var layerMask = 1 << _obstacleLayer;

            if (!Physics.BoxCast(
                    _gameplaySphere.position,
                    halfExtents,
                    Vector3.back,
                    out _,
                    Quaternion.identity,
                    Mathf.Infinity,
                    layerMask))
            {
                Debug.Log("WIN!!!!");
                
                _gameManager.OnWin();
                
                _projectileHit.OnAfterObstaclesDestroyed -= CheckDoor;
                _projectileHit = null;
                
                return;
            }
            
            Debug.Log("NOT WIN");

            _projectileHit.OnAfterObstaclesDestroyed -= CheckDoor;
            _projectileHit = null;
        }

        private void OnDisable()
        {
            if (_projectileHit != null)
            {
                _projectileHit.OnAfterObstaclesDestroyed -= CheckDoor;
                _projectileHit = null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_gameplaySphere == null)
                return;

            var sphereScale = _gameplaySphere.localScale.x;

            var halfExtents = new Vector3(
                sphereScale * 0.5f,
                sphereScale * 0.5f,
                0.01f);

            Gizmos.DrawWireCube(
                _gameplaySphere.position,
                halfExtents * 2f);
        }
    }
}