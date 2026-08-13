using UnityEngine;

namespace Entities
{
    public class GameplaySphereMoveAttribute : MonoBehaviour
    {
        [SerializeField] private GameplaySphere _gameplaySphere;

        public void Move(Vector2 screenPosition)
        {
            var camera = Camera.main;

            var sphereScreenPosition =
                camera.WorldToScreenPoint(_gameplaySphere.transform.position);

            sphereScreenPosition.x = screenPosition.x;

            var worldPosition =
                camera.ScreenToWorldPoint(sphereScreenPosition);

            _gameplaySphere.transform.position = new Vector3(
                worldPosition.x,
                _gameplaySphere.transform.position.y,
                _gameplaySphere.transform.position.z);
        }

        public void Release()
        {
        }
    }
}