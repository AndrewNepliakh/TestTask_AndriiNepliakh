using UnityEngine;

namespace Entities
{
    public class ObstacleHitAttribute : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _hitMaterial;

        private void OnEnable()
        {
            Reset();
        }

        public void SetHit()
        {
            _renderer.material = _hitMaterial;
        }

        public void Reset()
        {
            _renderer.material = _defaultMaterial;
        }
    }
}