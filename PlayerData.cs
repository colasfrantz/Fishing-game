using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerData : Node
{
	public static PlayerData Instance {get; set;}
	public string CurrentLocation {get ; set;} = "Pakko Beach";
	
	public List<FishData> CaughtFishes = Enumerable.Repeat<FishData>(null, 20).ToList();
	
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
		for (int i = 0; i < CaughtFishes.Count; i++)
		{
			if (CaughtFishes[i] == null)
			{
				CaughtFishes[i] = fish;
				GD.Print($"Poisson {fish.Name} added at index {i}");
				return;
			}
		}
		GD.Print("Inventaire plein !");
	}
}
