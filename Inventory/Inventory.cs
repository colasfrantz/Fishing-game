using Godot;
using System.Collections.Generic;

public partial class Inventory : Control
{
	[Export] public PackedScene SlotScene;
	private GridContainer _grid;
	private Button _menu;
	private Button stats;

	public override void _Ready()
	{
		_grid = GetNode<GridContainer>("VBoxContainer/ScrollContainer/Grid");
		_menu = GetNode<Button>("Menu");
		stats = GetNode<Button>("stat");
		
		_menu.Pressed += OnMenuPressed;
		stats.Pressed += OnStatsPressed;
		UpdateDisplay();
	}

	public void UpdateDisplay()
	{
		var fishes = PlayerData.Instance.CaughtFishes;
		var slots = GetNode<GridContainer>("VBoxContainer/ScrollContainer/Grid").GetChildren();

		for (int i = 0; i < slots.Count; i++)
		{
			if (slots[i] is InventorySlot slot)
			{
				// On envoie le poisson (ou null) à la case i
				slot.Display(fishes[i], i);
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
