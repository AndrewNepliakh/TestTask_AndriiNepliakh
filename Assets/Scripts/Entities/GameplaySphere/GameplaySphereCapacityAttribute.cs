using System;
using Zenject;
using Managers;
using DG.Tweening;
using UnityEngine;

namespace Entities
{
    public class GameplaySphereCapacityAttribute : MonoBehaviour, IInitializer
    {
        [Inject] private ILevelManager _levelManager;
        [Inject] private IGameManager _gameManager;

        [SerializeField] private GameplaySphereTapAttribute _tapAttribute;
        [SerializeField] private Transform _scaleTransform;
        [SerializeField] private float _shrinkTimePerCapacity = 1f;

        [SerializeField, Range(0f, 1f)]
        private float _loseCapacityPercent = 0.1f;
        
        private const float MinSpentCapacity = 0.01f;

        private float _capacity;
        private float _initialCapacity;
        private float _capacityBeforeShrink;

        private Tween _shrinkTween;

        public float SpentCapacity { get; private set; }

        public event Action<float> OnCapacitySpent;
        public event Action<float> OnScaleChanged;

        private void OnEnable()
        {
            _tapAttribute.OnPointerDownEvent += StartShrink;
            _tapAttribute.OnPointerUpEvent += StopShrink;
        }

        public void Initialize()
        {
            _initialCapacity = _levelManager
                .GetLevelConfigOfCurrentLevel()
                .Capacity;

            _capacity = _initialCapacity;

            SetScale(_capacity);
        }

        private void StartShrink()
        {
            _shrinkTween?.Kill();

            _capacityBeforeShrink = _capacity;

            _shrinkTween = DOTween.To(
                    () => _scaleTransform.localScale.x,
                    SetScale,
                    0f,
                    _capacity * _shrinkTimePerCapacity)
                .SetEase(Ease.Linear);
        }

        private void StopShrink()
        {
            _shrinkTween?.Kill();
            _shrinkTween = null;

            var currentScale = _scaleTransform.localScale.x;

            SpentCapacity = Mathf.Max(
                0f,
                _capacityBeforeShrink - currentScale);

            _capacity = currentScale;

            if (SpentCapacity <= MinSpentCapacity)
            {
                SpentCapacity = 0f;
                return;
            }

            OnCapacitySpent?.Invoke(SpentCapacity);
        }

        private void CheckLose()
        {
            var loseThreshold = _initialCapacity * _loseCapacityPercent;

            Debug.Log($"_capacity: {_capacity},  lose: {loseThreshold}");
            
            if (_capacity < loseThreshold)
            {
                _gameManager.OnLose();
            }
        }

        private void SetScale(float value)
        {
            _scaleTransform.localScale = Vector3.one * value;

            var position = _scaleTransform.localPosition;
            position.y = value;

            _scaleTransform.localPosition = position;

            OnScaleChanged?.Invoke(value);
        }

        private void OnDisable()
        {
            if (_tapAttribute != null)
            {
                _tapAttribute.OnPointerDownEvent -= StartShrink;
                _tapAttribute.OnPointerUpEvent -= StopShrink;
            }

            _shrinkTween?.Kill();
            _shrinkTween = null;
        }
    }
}