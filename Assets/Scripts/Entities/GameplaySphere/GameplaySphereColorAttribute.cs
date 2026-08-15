using UnityEngine;

namespace Entities
{
    public class GameplaySphereColorAttribute : MonoBehaviour, IInitializer
    {
        [SerializeField] private GameplaySphereTapAttribute _gameplaySphereTapAttribute;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _greyMaterial;


        public void Initialize()
        {
            _gameplaySphereTapAttribute.OnCanReceiveTapChanged += OnCanReceiveTapChanged;
            
            _renderer.material = _defaultMaterial;
        }

        private void OnCanReceiveTapChanged(bool value)
        {
            _renderer.material = value ? _defaultMaterial : _greyMaterial;
        }

        private void OnDisable()
        {
            _gameplaySphereTapAttribute.OnCanReceiveTapChanged -= OnCanReceiveTapChanged;
        }
    }
}