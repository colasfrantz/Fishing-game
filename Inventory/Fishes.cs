using Godot;
using System;

public partial class Fishes : Control
{
	private Button _menu;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_menu = GetNode<Button>("VBoxContainer/menu");
		
		_menu.Pressed += OnMenuPressed;
	}

	private void OnMenuPressed()
	{
		GD.Print("Menu");
		GetTree().ChangeSceneToFile("res://main_menu.tscn");
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
