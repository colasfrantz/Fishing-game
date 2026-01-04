using Godot;
using System;
using System.Collections.Generic;

public partial class MapScript : Control
{
	private Label _curlocation;
	
	private Button _menu;
	
	private RichTextLabel _description;
	
	private Dictionary<string, string> _locations = new()
	{
		{"LakeButton", "Lake"},
		{"PakoBeachButton", "Pako Beach"},
		{"Lilie'sButton", "Lilie's"},
		{"RiverButton", "River"},
	};
	public override void _Ready()
	{
		_curlocation = GetNode<Label>("Location");
		_menu = GetNode<Button>("VBoxContainer/Menu");
		_description = GetNode<RichTextLabel>("Description");
		
		_menu.Pressed += OnMenuPressed;
		
		foreach(var pair in _locations)
		{
			var button = GetNode<Button>(pair.Key);
			button.Pressed += () => OnLocationSelected(pair.Value);
		}
		UpdateLocationLabel();
	}

	private void OnLocationSelected(string location)
	{
		PlayerData.Instance.CurrentLocation = location;
		GD.Print($"Moved to {location}");
		UpdateLocationLabel();
	}
	
	private void OnMenuPressed()
	{
		GD.Print("Menu");
		GetTree().ChangeSceneToFile("res://main_menu.tscn");
	}
	private void UpdateLocationLabel()
	{
		_curlocation.Text = $"You are at the {PlayerData.Instance.CurrentLocation}";
		switch ( PlayerData.Instance.CurrentLocation)
		{
			case "Lilie's" : 
				_description.Text = "At Lilie's, you will find a peaceful area where friendly fishes like to rest.";
				break;
			case "River" :
				_description.Text = "Only wild and rapid fishes live in the rivers !";
				break;
			case "Pako Beach" :
				_description.Text = "desc for Pako";
				break;
			case "Lake" :
				_description.Text = "desc for Lake";
				break;
		}
	}
	
	public override void _Process(double delta)
	{
	}
}
