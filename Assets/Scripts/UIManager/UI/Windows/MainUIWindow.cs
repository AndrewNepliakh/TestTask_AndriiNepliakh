using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using Managers;
using Zenject;

public class MainUIWindow : Window
{
	[Inject] private PoolManager _poolManager;
	[Inject] private IUIManager _uiManager;
	[Inject] private HttpClientHelper _httpClientHelper;

	[SerializeField] private Button _createButton;
	[SerializeField] private Button _deleteButton;
	[SerializeField] private Button _updateButton;
	[SerializeField] private Button _refreshButton;

	[SerializeField] private Transform _content;

	private List<ButtonModel> _buttonModels = new();
	private List<IButtonView> _buttonsViews = new();

	public override async void Show(UIViewArguments arguments)
	{
		base.Show(arguments);
		await SetupListOfButtons();

		_createButton.onClick.AddListener(OnCreateButtonClicked);
		_deleteButton.onClick.AddListener(OnDeleteButtonClicked);
		_refreshButton.onClick.AddListener(OnRefreshButton);
		_updateButton.onClick.AddListener(OnUpdateButton);
	}

	public override void Hide(UIViewArguments arguments)
	{
		base.Hide(arguments);
		_createButton.onClick.RemoveListener(OnCreateButtonClicked);
		_deleteButton.onClick.RemoveListener(OnDeleteButtonClicked);
		_updateButton.onClick.RemoveListener(OnUpdateButton);
		_updateButton.onClick.RemoveListener(OnUpdateButton);
	}

	public async void Refresh(int index = 0)
	{
		await SetupListOfButtons(index);
	}

	public async void UpdateData(int index)
	{
		try
		{
			await _httpClientHelper.PutRequest(index);
			await SetupListOfButtons();
		}
		catch (Exception e)
		{
			Debug.LogError($"There is no such item.... {e.Message}");
		}
	}

	public async void DeleteRequest(int index)
	{
		try
		{
			await _httpClientHelper.DeleteRequest(index);
			await SetupListOfButtons();
		}
		catch (Exception e)
		{
			Debug.LogError($"There is no such item.... {e.Message}");
		}
	}

	private async void OnCreateButtonClicked()
	{
		try
		{
			await _httpClientHelper.PostRequest();
			await SetupListOfButtons();
		}
		catch (Exception e)
		{
			Debug.LogError($"There is no such item.... {e.Message}");
		}
	}

	private async Task SetupListOfButtons(int index = 0)
	{
		if (index == 0)
		{
			await UpdateAll();
		}
		else
		{
			await UpdateSingle(index);
		}
	}

	private async Task UpdateAll()
	{
		try
		{
			_buttonModels = await _httpClientHelper.GetRequest<List<ButtonModel>>();
			_buttonsViews.ForEach(x => _poolManager.Release(x as ButtonView));
			_buttonsViews.Clear();

			foreach (var buttonModel in _buttonModels)
			{
				IButtonView button;
				button = await _poolManager.GetOrCreate<ButtonView>(_content);
				button.Initialize(int.Parse(buttonModel.ID), 0, _buttonModels, _buttonsViews);
				_buttonsViews.Add(button);
			}

			_buttonsViews.OrderBy(x => x.Id);
			_buttonsViews.ForEach(x => x.Transform.SetSiblingIndex(x.Id));
			_buttonsViews.ForEach(x => x.Refresh());
		}
		catch (Exception e)
		{
			Debug.LogError($"There is no such item.... {e.Message}");
		}
	}

	private async Task UpdateSingle(int index)
	{
		try
		{
			var buttonModel = await _httpClientHelper.GetRequest<ButtonModel>(index);
			var buttonIndex = _buttonModels.FindIndex(x => int.Parse(x.ID) == index);
			_buttonModels[buttonIndex] = buttonModel;
			var button = _buttonsViews[buttonIndex];
			button.Initialize(int.Parse(buttonModel.ID), index, _buttonModels, _buttonsViews);
			button.Refresh();
		}
		catch (Exception e)
		{
			Debug.LogError($"There is no such item.... {e.Message}");
		}
	}

	private async void OnRefreshButton() =>
		await _uiManager.ShowPopupWithDI<InputPopup>(new UIViewArguments {FromAction = Enums.FromAction.REFRESH});

	private async void OnUpdateButton() =>
		await _uiManager.ShowPopupWithDI<InputPopup>(new UIViewArguments {FromAction = Enums.FromAction.UPDATE});

	private async void OnDeleteButtonClicked() =>
		await _uiManager.ShowPopupWithDI<InputPopup>(new UIViewArguments {FromAction = Enums.FromAction.DELETE});

	public override void Reset()
	{
	}
}