using Managers;

public interface IInputManager
{
    void RegisterEntireDraggable(IEntireDraggable draggable);
    void UnregisterEntireDraggable(IEntireDraggable draggable);
}