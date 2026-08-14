using Zenject;
using Managers;
using DG.Tweening;
using UnityEngine;

namespace Entities
{
    public class GameplaySphereWinAnimationAttribute : MonoBehaviour
    {
        [Inject] private IGameManager _gameManager;
        [Inject] private IGameplaySphereManager _gameplaySphereManager;

        [SerializeField] private Transform _gameplaySphere;

        [SerializeField] private float _duration = 4f;
        [SerializeField] private float _jumpDistance = 2f;
        [SerializeField] private float _jumpHeight = 1f;
        [SerializeField] private int _jumpCount = 8;

        private Tween _animationTween;

        private void OnEnable()
        {
            _gameManager.OnStateChange += OnGameStateChanged;
        }

        private void OnDisable()
        {
            _gameManager.OnStateChange -= OnGameStateChanged;

            _animationTween?.Kill();
            _animationTween = null;
        }

        private void OnGameStateChanged(GameState state)
        {
            if (state != GameState.Win)
                return;

            Play();
        }

        private void Play()
        {
            if (_gameplaySphere == null)
                return;

            _animationTween?.Kill();

            var startPosition = _gameplaySphere.position;

            var endPosition =
                startPosition +
                Vector3.back * (_jumpDistance * _jumpCount);

            _animationTween = _gameplaySphere
                .DOJump(
                    endPosition,
                    _jumpHeight,
                    _jumpCount,
                    _duration)
                .SetEase(Ease.Linear)
                .OnComplete(Despawn);
        }

        private void Despawn()
        {
            _animationTween = null;

            _gameplaySphereManager.DespawnGameplaySphere();
        }
    }
}