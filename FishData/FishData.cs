using Godot;
using System;

public enum FishRarity
{
	Common,
	Uncommon,
	Rare,
	Epic,
	Legendary
}
[GlobalClass]
public partial class FishData : Resource
{
	
	public string Name { get; set;}
	public float Weight { get; set;}
	public float Length  { get; set;}
	public string Color { get; set;}
	public FishRarity Rarity { get; set;}
	public string Temperament {get;set;}
	public string Species {get;set;}
	[Export] public Texture2D  Image {get;set;} 
	public string Descritpion {get;set;}
	
	public FishData (string name, string species, float weight, float length, string color, FishRarity rarity, string temperament)
	{
		Name =name;
		Species = species;
		Weight = weight;
		Length = length;
		Color = color;
		Rarity = rarity;
		Temperament = temperament;
	}
}
