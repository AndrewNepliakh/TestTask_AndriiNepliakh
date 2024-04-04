using System;
using Newtonsoft.Json;


[Serializable]
public class ButtonModel
{
	
	[JsonProperty("text")] 
	public string Name { get; set; }
	[JsonProperty("color")] 
	public string Color { get; set; }
	[JsonProperty("animationType")] 
	public bool AnimationType { get; set; }
	[JsonProperty("id")] 
	public string ID { get; set; }
	
}