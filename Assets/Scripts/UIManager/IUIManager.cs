using System.Threading.Tasks;
using UnityEngine;

namespace Managers
{
    public interface IUIManager
    {
        Canvas MainCanvas { get; }

        Window CurrentWindow { get; }

        Task<T> ShowWindowWithDI<T>(UIViewArguments args = null)
            where T : Window;

        Task<T> ShowPopupWithDI<T>(UIViewArguments args = null)
            where T : Popup;

        void HideWindow(UIViewArguments args = null);
        void HidePopup(UIViewArguments args = null);
        void UnloadWindows();
    }
}