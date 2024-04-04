using Managers;
using UnityEngine;
using Zenject;

public class MainSceneController : MonoBehaviour
{
	[Inject] private IUIManager _uiManager;
	private async void Start()
	{
		await _uiManager.ShowWindowWithDI<MainUIWindow>();
	}
}
