using Zenject;
using Managers;
using UnityEngine;

namespace Entities
{
    public class Track : MonoBehaviour
    {
        [Inject] private IGameplaySphereManager _gameplaySphereManager;

        [SerializeField] private Transform _scaleTransform;

        private GameplaySphereCapacityAttribute _capacityAttribute;

        private void OnEnable()
        {
            _gameplaySphereManager.OnGameplaySphereCreated += OnGameplaySphereCreated;

            if (_gameplaySphereManager.GameplaySphere != null)
                OnGameplaySphereCreated(_gameplaySphereManager.GameplaySphere);
        }

        private void OnDisable()
        {
            _gameplaySphereManager.OnGameplaySphereCreated -= OnGameplaySphereCreated;

            UnsubscribeFromCapacity();
        }

        private void OnGameplaySphereCreated(GameplaySphere gameplaySphere)
        {
            UnsubscribeFromCapacity();

            _capacityAttribute =
                gameplaySphere.GetComponent<GameplaySphereCapacityAttribute>();

            if (_capacityAttribute == null)
                return;

            _capacityAttribute.OnScaleChanged += OnScaleChanged;

            OnScaleChanged(
                gameplaySphere.transform.localScale.x);
        }

        private void OnScaleChanged(float value)
        {
            var scale = _scaleTransform.localScale;
            scale.x = value;

            _scaleTransform.localScale = scale;
        }

        private void UnsubscribeFromCapacity()
        {
            if (_capacityAttribute == null)
                return;

            _capacityAttribute.OnScaleChanged -= OnScaleChanged;
            _capacityAttribute = null;
        }
    }
}