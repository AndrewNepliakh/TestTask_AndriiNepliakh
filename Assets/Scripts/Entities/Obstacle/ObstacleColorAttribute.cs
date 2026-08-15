using UnityEngine;

namespace Entities
{
    public class ObstacleColorAttribute : MonoBehaviour, IInitializer
    {
        [SerializeField] private ObstacleHitAttribute _obstacleHitAttribute;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _hitMaterial;


        public void Initialize()
        {
            _obstacleHitAttribute.OnHit += OnHit;
            
            _renderer.material = _defaultMaterial;
        }

        private void OnHit()
        {
            _renderer.material = _hitMaterial;
        }

        private void OnDisable()
        {
            _obstacleHitAttribute.OnHit -= OnHit;
        }
    }
}