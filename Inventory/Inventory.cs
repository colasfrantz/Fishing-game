using Godot;
using System.Collections.Generic;

public partial class Inventory : Control
{
	[Export] public PackedScene SlotScene;
	private GridContainer _grid;
	private Button _menu;

	public override void _Ready()
	{
		_grid = GetNode<GridContainer>("VBoxContainer/ScrollContainer/Grid");
		_menu = GetNode<Button>("Menu");
		
		_menu.Pressed += OnMenuPressed;
		DisplayInventory();
	}

	public void DisplayInventory()
	{
		// On vide la grille
		foreach (Node child in _grid.GetChildren()) child.QueueFree();

		var caughtFishes = PlayerData.Instance.CaughtFishes;

		// On crée toujours 20 cases (ton maximum)
		for (int i = 0; i < 20; i++)
		{
			var slot = SlotScene.Instantiate<InventorySlot>();
			_grid.AddChild(slot);

			if (i < caughtFishes.Count)
			{
				slot.Display(caughtFishes[i], i);
			}
			else
			{
				slot.Display(null, i); // Case vide
			}
		}
	}
	
	public void OnMenuPressed(){
		GD.Print("Going back to the menu !");
		GetTree().ChangeSceneToFile("res://main_menu.tscn");
	}
}
