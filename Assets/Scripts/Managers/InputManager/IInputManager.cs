namespace Managers
{
    public interface IInputManager
    {
        void RegisterEntireDraggable(IEntireDraggable draggable);
        void UnregisterEntireDraggable(IEntireDraggable draggable);

        void RegisterTappable(ITappable tappable);
        void UnregisterTappable(ITappable tappable);
    }
}