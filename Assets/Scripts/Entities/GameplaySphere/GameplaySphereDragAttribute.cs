using Managers;
using UnityEngine;
using Zenject;

namespace Entities.GameplaySphere
{
    public class GameplaySphereDragAttribute : MonoBehaviour, IEntireDraggable
    {
        [Inject] private IInputManager _inputManager;

        [SerializeField] private GameplaySphereMoveAttribute _moveAttribute;

        private void OnEnable()
        {
            _inputManager.RegisterEntireDraggable(this);
        }

        private void OnDisable()
        {
            _inputManager.UnregisterEntireDraggable(this);
        }

        public void OnDrag(Vector2 screenPosition)
        {
            _moveAttribute.Move(screenPosition);
        }

        public void OnRelease()
        {
            _moveAttribute.Release();
        }
    }
}