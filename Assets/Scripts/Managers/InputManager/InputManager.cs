using System;
using Zenject;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : IInputManager, IInitializable, IDisposable
    {
        private Camera _camera;

        private readonly GameplayInput _input = new();

        private IEntireDraggable _entireDraggable;

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
        }

        private void OnTap(InputAction.CallbackContext _)
        {
            if (!_isPressed)
                return;

            if (_camera == null)
                _camera = Camera.main;

            var screenPosition =
                _input.Gameplay.PointerPosition.ReadValue<Vector2>();

            var ray = _camera.ScreenPointToRay(screenPosition);

            if (!Physics.Raycast(ray, out var hit))
                return;

            if (hit.collider.TryGetComponent<ITappable>(out var tappable))
            {
                tappable.OnTap();
            }
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
    }
}