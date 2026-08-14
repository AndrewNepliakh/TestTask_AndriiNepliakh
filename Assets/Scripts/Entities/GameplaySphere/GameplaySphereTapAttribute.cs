using System;
using Zenject;
using Managers;
using UnityEngine;

namespace Entities
{
    public class GameplaySphereTapAttribute : MonoBehaviour, ITappable
    {
        [Inject] private IInputManager _inputManager;

        private bool _canReceiveTap;
        private bool _isPointerDown;

        public event Action OnTapEvent;
        public event Action OnPointerDownEvent;
        public event Action OnPointerUpEvent;

        public void SetCanReceiveTap(bool value)
        {
            _canReceiveTap = value;

            if (!value)
                _isPointerDown = false;
        }

        private void OnEnable()
        {
            SetCanReceiveTap(true);
            _inputManager.RegisterTappable(this);
        }

        private void OnDisable()
        {
            _inputManager.UnregisterTappable(this);

            _isPointerDown = false;
            _canReceiveTap = false;
        }

        public void OnPointerDown()
        {
            if (!_canReceiveTap)
                return;

            _isPointerDown = true;

            OnPointerDownEvent?.Invoke();
        }

        public void OnPointerUp()
        {
            if (!_canReceiveTap)
                return;

            if (!_isPointerDown)
                return;

            _isPointerDown = false;

            OnPointerUpEvent?.Invoke();
        }

        public void OnTap()
        {
            if (!_canReceiveTap)
                return;

            if (!_isPointerDown)
                return;

            OnTapEvent?.Invoke();
        }
    }
}