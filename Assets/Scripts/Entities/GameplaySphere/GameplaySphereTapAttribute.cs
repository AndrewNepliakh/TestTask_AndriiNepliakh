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

        public event Action OnTapEvent;
        public event Action OnPointerDownEvent;
        public event Action OnPointerUpEvent;
        
        public void SetCanReceiveTap(bool value)
        {
            _canReceiveTap = value;
        }

        private void OnEnable()
        {
            SetCanReceiveTap(true);
            _inputManager.RegisterTappable(this);
        }

        private void OnDisable()
        {
            _inputManager.UnregisterTappable(this);
            
            SetCanReceiveTap(false);
        }

        public void OnPointerDown()
        {
            if (!_canReceiveTap)
                return;

            OnPointerDownEvent?.Invoke();
        }

        public void OnPointerUp()
        {
            if (!_canReceiveTap)
                return;

            OnPointerUpEvent?.Invoke();
        }

        public void OnTap()
        {
            if (!_canReceiveTap)
                return;

            OnTapEvent?.Invoke();
        }
    }
}