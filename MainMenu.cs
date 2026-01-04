using Godot;
using System;

public partial class MainMenu : Control
{
	private Button _playButton;
	private Button _fishesButton;
	private Button _mapButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playButton = GetNode<Button>("VBoxContainer/PlayButton");
		_fishesButton = GetNode<Button>("VBoxContainer/Inventory");
		_mapButton = GetNode<Button>("VBoxContainer/Map");
		
		_playButton.Pressed += OnPlayPressed;
		_fishesButton.Pressed += OnFishesPressed;
		_mapButton.Pressed += OnMapPressed;
		
	}

	private void OnPlayPressed()
	{
		GD.Print("Play !");
		GetTree().ChangeSceneToFile("res://Game/FishingGame.tscn");
	}

	private void OnMapPressed()
	{
		GD.Print("Opening Map !");
		GetTree().ChangeSceneToFile("res://Map/MapScene.tscn");
	}
	
	private void OnFishesPressed()
	{
		GD.Print("Let's see our Fishes !");
		GetTree().ChangeSceneToFile("res://Inventory/Inventory.tscn");
	}
}
