using System;
using Zenject;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : IInputManager, IInitializable, IDisposable
    {
        private readonly GameplayInput _input = new();

        private IEntireDraggable _entireDraggable;
        private ITappable _tappable;

        private bool _isPressed;

        public void Initialize()
        {
            _input.Enable();

            _input.Gameplay.Tap.started += OnPointerDown;
            _input.Gameplay.Tap.performed += OnTap;
            _input.Gameplay.Tap.canceled += OnPointerUp;

            _input.Gameplay.Drag.performed += OnDrag;
        }

        public void Dispose()
        {
            _input.Gameplay.Tap.started -= OnPointerDown;
            _input.Gameplay.Tap.performed -= OnTap;
            _input.Gameplay.Tap.canceled -= OnPointerUp;

            _input.Gameplay.Drag.performed -= OnDrag;

            _input.Disable();
        }

        private void OnPointerDown(InputAction.CallbackContext _)
        {
            _isPressed = true;

            _tappable?.OnPointerDown();
        }

        private void OnTap(InputAction.CallbackContext _)
        {
            if (!_isPressed)
                return;

            _tappable?.OnTap();
        }

        private void OnDrag(InputAction.CallbackContext _)
        {
            if (!_isPressed || _entireDraggable == null)
                return;

            var screenPosition =
                _input.Gameplay.PointerPosition.ReadValue<Vector2>();

            _entireDraggable.OnDrag(screenPosition);
        }

        private void OnPointerUp(InputAction.CallbackContext _)
        {
            if (!_isPressed)
                return;

            _isPressed = false;

            _tappable?.OnPointerUp();

            _entireDraggable?.OnRelease();
        }

        public void RegisterEntireDraggable(IEntireDraggable draggable)
        {
            _entireDraggable = draggable;
        }

        public void UnregisterEntireDraggable(IEntireDraggable draggable)
        {
            if (_entireDraggable == draggable)
                _entireDraggable = null;
        }

        public void RegisterTappable(ITappable tappable)
        {
            _tappable = tappable;
        }

        public void UnregisterTappable(ITappable tappable)
        {
            if (_tappable == tappable)
                _tappable = null;
        }
    }
}