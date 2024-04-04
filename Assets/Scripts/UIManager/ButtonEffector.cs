using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ButtonEffector : MonoBehaviour
{
	public bool IsOnAwake;

	[SerializeField] private GameObject _explosionAnivation;

	private RectTransform _rectTransform;
	private float _fadeTime = 0.3f;
	private Vector3 _startScale;
	private Quaternion _startRotation;

	private void OnEnable()
	{
		if (IsOnAwake)
			OnClick();
	}
	
	private void Start()
	{
		_rectTransform = GetComponent<RectTransform>();
		_startScale = _rectTransform.localScale;
		_startRotation = _rectTransform.rotation;
	}

	public void OnClick()
	{
		var type = Random.Range(0, Enum.GetValues(typeof(Enums.EffectsType)).Length);

		switch (type)
		{
			case 0:
				Jump();
				break;
			case 1:
				Shake();
				break;
			case 2:
				Bounce();
				break;
		}
		
		if(_explosionAnivation != null) _explosionAnivation.SetActive(true);
	}

	private void Jump()
	{
		_rectTransform.DOScale(_startScale * 1.2f, _fadeTime / 2)
			.SetEase(Ease.OutBounce)
			.OnComplete(() =>
			{
				_rectTransform.DOScale(_startScale, _fadeTime)
					.SetEase(Ease.InExpo);
			});
	}
	
	private void Shake()
	{
		_rectTransform.DOShakePosition(_fadeTime * 3, new Vector3(4, 0, 0), 10, 1)
			.OnComplete(() =>
			{
				_rectTransform.DOScale(_startScale, _fadeTime / 2)
					.SetEase(Ease.InExpo);
			});
	}
	
	private void Bounce()
	{
		_rectTransform.DOShakeScale(_fadeTime * 3, Vector3.one * 0.1f, 5, 1)
			.SetEase(Ease.OutBounce)
			.OnComplete(() =>
			{
				_rectTransform.DOScale(_startScale, _fadeTime)
					.SetEase(Ease.InExpo);
			});
	}
}
