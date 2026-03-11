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
		UpdateDisplay();
	}

	public void UpdateDisplay()
	{
		var fishes = PlayerData.Instance.CaughtFishes;
		var slots = _grid.GetChildren();
		
		for(int i=0; i<slots.Count; i++){
			if(slots[i] is InventorySlot slot){
				if (i < fishes.Count)
				{
					slot.Display(fishes[i], i);
				}
			}
		}
	}
	
	public void OnMenuPressed(){
		GD.Print("Going back to the menu !");
		GetTree().ChangeSceneToFile("res://main_menu.tscn");
	}
}
