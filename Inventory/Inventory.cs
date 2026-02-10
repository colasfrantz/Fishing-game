using Godot;
using System;

public partial class Inventory : Control
{
	private Button _menu;
	private GridContainer _grid;
	
	// Il te faudra une référence vers la scène du slot qu'on a créé étape 3
	// Charge-le dans l'inspecteur ou via le chemin
	private PackedScene _slotScene = GD.Load<PackedScene>("res://Inventory/InventorySlot.tscn"); // Vérifie le chemin !

	public override void _Ready()
	{
		_menu = GetNode<Button>("VBoxContainer/Menu");
		// Récupère ton GridContainer. Adapte le chemin selon ta scène.
		_grid = GetNode<GridContainer>("VBoxContainer/Grid"); 
		
		_menu.Pressed += OnMenuPressed;

		DisplayInventory();
	}

	private void DisplayInventory()
	{
		// 1. Nettoyer l'existant (au cas où)
		foreach (Node child in _grid.GetChildren())
		{
			child.QueueFree();
		}

		// 2. Configurer la grille
		_grid.Columns = 5; // Par exemple, 5 poissons par ligne

		// 3. Créer un slot pour chaque poisson
		var fishes = PlayerData.Instance.CaughtFishes;
		for (int i = 0; i < fishes.Count; i++)
		{
			var fishData = fishes[i];
			
			// Instancier le slot
			InventorySlot newSlot = _slotScene.Instantiate<InventorySlot>();
			_grid.AddChild(newSlot);
			
			// Configurer le slot
			newSlot.Setup(fishData, i);
		}
	}

	private void OnMenuPressed()
	{
		GD.Print("Menu");
		GetTree().ChangeSceneToFile("res://main_menu.tscn");
	}
}
