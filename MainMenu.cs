using Godot;
using System;

public partial class MainMenu : Control
{
	private Button _playButton;
	private Button _fishesButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playButton = GetNode<Button>("VBoxContainer/PlayButton");
		_fishesButton = GetNode<Button>("VBoxContainer/Fishes");
		
		_playButton.Pressed += OnPlayPressed;
		_fishesButton.Pressed += OnFishesPressed;
		
	}

	private void OnPlayPressed()
	{
		GD.Print("Play !");
		GetTree().ChangeSceneToFile("res://Game/FishingGame.tscn");
	}
	
	private void OnFishesPressed()
	{
		GD.Print("Fishes !");
		GetTree().ChangeSceneToFile("res://Inventory/Fishes.tscn");
	}
}
