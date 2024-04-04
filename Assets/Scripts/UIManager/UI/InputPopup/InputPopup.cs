using UnityEngine.UI;
using UnityEngine;
using Managers;
using Zenject;
using System;
using TMPro;

public class InputPopup : Popup
{
	[Inject] private IUIManager _uiManager;

	[SerializeField] private TMP_InputField _inputField;
	[SerializeField] private TMP_Text _attentionText;
	[SerializeField] private Button _bgCloseButton;
	[SerializeField] private Button _closeButton;
	[SerializeField] private Button _acceptButton;
	[SerializeField] private Button _sendButton;

	private Enums.FromAction _fromAction;

	public override void Show(UIViewArguments arguments)
	{
		
		
		
		
		base.Show(arguments);

		_fromAction = arguments.FromAction;

		_acceptButton.onClick.AddListener(OnAcceptButtonClicked);
		_closeButton.onClick.AddListener(OncloseButtonClicked);
		_bgCloseButton.onClick.AddListener(OncRejectClicked);
		_sendButton.onClick.AddListener(OnSendButtonClicked);

		var isFromRefresh = _fromAction == Enums.FromAction.REFRESH;

		_attentionText.gameObject.SetActive(isFromRefresh);
		_acceptButton.gameObject.SetActive(isFromRefresh);
		_closeButton.gameObject.SetActive(isFromRefresh);
		_inputField.gameObject.SetActive(!isFromRefresh);
		_sendButton.gameObject.SetActive(!isFromRefresh);
	}

	private void OnAcceptButtonClicked()
	{
		_attentionText.gameObject.SetActive(false);
		_acceptButton.gameObject.SetActive(false);
		_closeButton.gameObject.SetActive(false);
		_inputField.gameObject.SetActive(true);
		_sendButton.gameObject.SetActive(true);
	}
	
	private void OncRejectClicked() => _uiManager.HidePopup();
	
	private void OncloseButtonClicked()
	{
		if (_fromAction == Enums.FromAction.REFRESH)
			((MainUIWindow) _uiManager.CurrentWindow).Refresh();
		_uiManager.HidePopup();
	}
	
	private void OnSendButtonClicked()
	{
		if (int.TryParse(_inputField.text, out var res))
		{
			switch (_fromAction)
			{
				case Enums.FromAction.DELETE:
					((MainUIWindow) _uiManager.CurrentWindow).DeleteRequest(res);
					break;
				case Enums.FromAction.UPDATE:
					((MainUIWindow) _uiManager.CurrentWindow).UpdateData(res);
					break;
				case Enums.FromAction.REFRESH:
					((MainUIWindow) _uiManager.CurrentWindow).Refresh(res);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		_uiManager.HidePopup();
	}

	public override void Hide(UIViewArguments arguments)
	{
		base.Hide(arguments);

		_acceptButton.onClick.RemoveListener(OnAcceptButtonClicked);
		_closeButton.onClick.RemoveListener(OncloseButtonClicked);
		_bgCloseButton.onClick.RemoveListener(OncRejectClicked);
		_sendButton.onClick.RemoveListener(OnSendButtonClicked);
	}
}