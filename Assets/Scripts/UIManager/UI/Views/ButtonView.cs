using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ButtonView : MonoBehaviour, IPoolable, IButtonView
{
	public int Id { get; set; }
	public Transform Transform => GetComponent<Transform>();
	public int? UpdateIndex { get; set; }

	[SerializeField] private ButtonEffector _buttonEffector;

	private TMP_Text _name;
	private Image _imageBg;

	private ButtonModel _currentData;
	private PoolManager _poolManager;
	private Transform _content;

	public void Initialize(int id, int index, List<ButtonModel> buttonModels, List<IButtonView> buttonViews)
	{
		Id = id;
		UpdateIndex = index;
		_currentData = buttonModels.Find(x => int.Parse(x.ID) == id);
		_name = GetComponentInChildren<TMP_Text>();
		_imageBg = GetComponent<Image>();
	}
	
	public void Refresh()
	{
		if (UpdateIndex != 0)
		{
			if (UpdateIndex != Id) return;
			SetValues();
		}
		else
		{
			SetValues();
		}
	}

	private void SetValues()
	{
		_name.text = Id + " " + _currentData.Name;
		_imageBg.color = GetColor(_currentData);
		_buttonEffector.OnClick();
	}

	private Color GetColor(ButtonModel btnModel) =>
		ColorUtility.TryParseHtmlString(btnModel.Color, out Color color)
			? color
			: Color.white;


	private bool IsAnimation(ButtonModel buttonModel)
	{
		return false;
	}

	public void OnActivate(object argument = default)
	{
		gameObject.SetActive(true);
	}

	public void OnDeactivate(object argument = default)
	{
		gameObject.SetActive(false);
	}
}