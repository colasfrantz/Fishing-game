using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerData : Node
{
	public static PlayerData Instance {get; set;}
	public string CurrentLocation {get ; set;} = "Pakko Beach";
	
	public List<FishData> CaughtFishes { get; private set; } = new List<FishData>();
	
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
	
	public void AddFish(FishData fish)
	{
		CaughtFishes.Add(fish);
		GD.Print($"Poisson added : {fish.Name}.\nTotal : {CaughtFishes.Count}.");
	}
}
