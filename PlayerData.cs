using Godot;
using System;

public partial class PlayerData : Node
{
	public static PlayerData Instance {get; set;}
	public string CurrentLocation {get ; set;} = "Pakko Beach";
	
	public override void _Ready()
	{
		if(Instance != null)
		{
			QueueFree();
			return;
		}
		Instance = this;
		ProcessMode = ProcessModeEnum.Always;
		GetTree().Root.AddChild(this);
	}
}
