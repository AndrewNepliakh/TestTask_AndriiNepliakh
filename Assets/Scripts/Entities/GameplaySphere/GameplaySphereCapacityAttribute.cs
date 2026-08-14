using System;
using Zenject;
using Managers;
using DG.Tweening;
using UnityEngine;

namespace Entities
{
    public class GameplaySphereCapacityAttribute : MonoBehaviour
    {
        [Inject] private ILevelManager _levelManager;

        [SerializeField] private GameplaySphereTapAttribute _tapAttribute;
        [SerializeField] private Transform _scaleTransform;
        [SerializeField] private float _shrinkTimePerCapacity = 1f;

        private float _capacity;
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

        private void Start()
        {
            _capacity = _levelManager
                .GetLevelConfigOfCurrentLevel()
                .Capacity;

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

            OnCapacitySpent?.Invoke(SpentCapacity);
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