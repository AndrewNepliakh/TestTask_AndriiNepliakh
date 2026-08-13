using UnityEngine;

public interface IEntireDraggable
{
    void OnDrag(Vector2 screenPosition);
    void OnRelease();
}