using Godot;
using System;

public partial class WinningScene : Control
{
	private Button _menu;
	private Button _fishes;
	
	
	public override void _Ready()
	{
		_menu = GetNode<Button>("VBoxContainer/Menu");
		_fishes = GetNode<Button>("VBoxContainer/Fishes");
		
		_menu.Pressed += OnMenuPressed;
		_fishes.Pressed += OnFishesPressed;
	}
	
	private void OnMenuPressed()
	{
		GD.Print("Go back to the Menu !");
		GetTree().ChangeSceneToFile("res://main_menu.tscn");
	}
	
	private void OnFishesPressed()
	{
		GD.Print("Let's see our Fishes !");
		GetTree().ChangeSceneToFile("res://Inventory/Fishes.tscn");
	}
	
	
	public override void _Process(double delta)
	{
	}
}
