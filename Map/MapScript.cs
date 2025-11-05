using Godot;
using System;
using System.Collections.Generic;

public partial class MapScript : Control
{
	private Label _curlocation;
	
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
	
	private void UpdateLocationLabel()
	{
		_curlocation.Text = $"You are at the {PlayerData.Instance.CurrentLocation}";
	}
	
	public override void _Process(double delta)
	{
	}
}
