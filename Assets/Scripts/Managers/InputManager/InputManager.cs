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

        private bool _isDragging;

        public void Initialize()
        {
            _input.Enable();

            _input.Gameplay.Tap.performed += OnTap;

            _input.Gameplay.Drag.started += OnDragStarted;
            _input.Gameplay.Drag.performed += OnDrag;
            _input.Gameplay.Drag.canceled += OnDragCanceled;
        }

        public void Dispose()
        {
            _input.Gameplay.Tap.performed -= OnTap;

            _input.Gameplay.Drag.started -= OnDragStarted;
            _input.Gameplay.Drag.performed -= OnDrag;
            _input.Gameplay.Drag.canceled -= OnDragCanceled;

            _input.Disable();
        }

        private void OnTap(InputAction.CallbackContext _)
        {
            if (_isDragging)
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

        private void OnDragStarted(InputAction.CallbackContext _)
        {
            if (_entireDraggable == null)
                return;

            _isDragging = true;

            var screenPosition =
                _input.Gameplay.PointerPosition.ReadValue<Vector2>();

            _entireDraggable.OnDrag(screenPosition);
        }

        private void OnDrag(InputAction.CallbackContext _)
        {
            if (!_isDragging || _entireDraggable == null)
                return;

            var screenPosition =
                _input.Gameplay.PointerPosition.ReadValue<Vector2>();

            _entireDraggable.OnDrag(screenPosition);
        }

        private void OnDragCanceled(InputAction.CallbackContext _)
        {
            if (!_isDragging)
                return;

            _isDragging = false;

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