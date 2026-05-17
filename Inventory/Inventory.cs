using Godot;
using System.Collections.Generic;

public partial class Inventory : Control
{
	private GridContainer _grid;
	private Button _menu;
	private Button stats;
	
	[Export] public PackedScene SlotScene;

	public override void _Ready()
	{
		// On cherche la grille de manière plus flexible
		_grid = FindChild("Grid", true, false) as GridContainer;
		
		if (_grid == null) {
			GD.PrintErr("ERREUR : Impossible de trouver le GridContainer !");
			return;
		}
		DisplayInventory();
		_menu = GetNode<Button>("Menu");
		stats = GetNode<Button>("stat");
		
		_menu.Pressed += OnMenuPressed;
		stats.Pressed += OnStatsPressed;
	}

	public void DisplayInventory()
{
	// 1. On nettoie la grille (pour éviter les doublons ou les nœuds vides de l'éditeur)
	foreach (Node child in _grid.GetChildren()) {
		child.QueueFree();
	}

	var caughtFishes = PlayerData.Instance.CaughtFishes;

	// 2. On crée les 20 cases à partir du fichier .tscn
	for (int i = 0; i < 20; i++) {
		var slot = SlotScene.Instantiate<InventorySlot>();
		_grid.AddChild(slot); // Ici, le slot aura AUTOMATIQUEMENT le FishIcon

		if (i < caughtFishes.Count) {
			slot.Display(caughtFishes[i], i);
		} else {
			slot.Display(null, i);
		}
	}
}
	
	public void OnMenuPressed(){
		GD.Print("Going back to the menu !");
		GetTree().ChangeSceneToFile("res://main_menu.tscn");
	}
	
	public void OnStatsPressed(){
		GD.Print($"{PlayerData.Instance.GetFishCount()}");
		for(int i = 0; i< PlayerData.Instance.GetFishCount();i++){
			GD.Print($"{PlayerData.Instance.CaughtFishes[i]}");
		}
	}
}
