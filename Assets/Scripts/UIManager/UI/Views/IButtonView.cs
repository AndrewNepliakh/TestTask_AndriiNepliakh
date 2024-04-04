using System;
using System.Collections.Generic;
using UnityEngine;

public interface IButtonView
{
	int Id { get; set; }
	Transform Transform { get; }
	void Refresh();
	void Initialize(int parse, int index, List<ButtonModel> buttonModels, List<IButtonView> buttonViews);
	
}