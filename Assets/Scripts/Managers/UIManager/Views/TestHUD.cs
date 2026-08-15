using Zenject;
using Managers;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TestHUD : Window
    {
        [Inject] private IGameplaySphereManager _gameplaySphereManager;

        [SerializeField] private Image _fillImage;
        [SerializeField] private Gradient _fillGradient;

        private GameplaySphereCapacityAttribute _gameplaySphereCapacityAttribute;
        private RectTransform _fillRectTransform;

        private float _initialScaleY;

        public override void Show(UIViewArguments arguments)
        {
            base.Show(arguments);

            _fillRectTransform = _fillImage.rectTransform;
            _initialScaleY = _fillRectTransform.localScale.y;

            _gameplaySphereManager.OnGameplaySphereCreated += OnGameplaySphereCreated;

            if (_gameplaySphereManager.GameplaySphere != null)
            {
                OnGameplaySphereCreated(_gameplaySphereManager.GameplaySphere);
            }
        }

        private void OnGameplaySphereCreated(GameplaySphere gameplaySphere)
        {
            if (_gameplaySphereCapacityAttribute != null)
            {
                _gameplaySphereCapacityAttribute.OnCapacityChanged -= OnCapacityChanged;
            }

            _gameplaySphereCapacityAttribute =
                gameplaySphere.GetComponent<GameplaySphereCapacityAttribute>();

            if (_gameplaySphereCapacityAttribute == null)
                return;

            _gameplaySphereCapacityAttribute.OnCapacityChanged += OnCapacityChanged;

            SetFillScale(_gameplaySphereCapacityAttribute.CapacityRatio);
        }

        private void OnCapacityChanged(float capacityRatio)
        {
            SetFillScale(capacityRatio);
        }

        private void SetFillScale(float value)
        {
            value = Mathf.Clamp01(value);

            var scale = _fillRectTransform.localScale;
            scale.y = _initialScaleY * value;

            _fillRectTransform.localScale = scale;

            _fillImage.color = _fillGradient.Evaluate(value);
        }

        public override void Hide()
        {
            base.Hide();

            _gameplaySphereManager.OnGameplaySphereCreated -= OnGameplaySphereCreated;

            if (_gameplaySphereCapacityAttribute != null)
            {
                _gameplaySphereCapacityAttribute.OnCapacityChanged -= OnCapacityChanged;

                _gameplaySphereCapacityAttribute = null;
            }

            SetFillScale(1f);
        }
    }
}