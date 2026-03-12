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
				GD.Print($"Succès ! Poisson ajouté à l'emplacement : {i}");
				return; // On s'arrête ici
			}
		}
		
		// Ce message ne s'affichera que si les 20 cases sont vraiment remplies
		GD.Print("Inventaire RÉELLEMENT plein !");
	}
	
	public int GetFishCount() => CaughtFishes.Count(f => f != null);
}
