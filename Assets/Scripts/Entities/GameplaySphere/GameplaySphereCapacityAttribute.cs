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
        private Tween _shrinkTween;

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

            _scaleTransform.localScale = Vector3.one * _capacity;

            var position = _scaleTransform.localPosition;
            position.y = _capacity;
            _scaleTransform.localPosition = position;
        }

        private void StartShrink()
        {
            _shrinkTween?.Kill();

            _shrinkTween = _scaleTransform
                .DOScale(
                    Vector3.zero,
                    _capacity * _shrinkTimePerCapacity)
                .SetEase(Ease.Linear);
        }

        private void StopShrink()
        {
            _shrinkTween?.Kill();
            _shrinkTween = null;
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