using Godot;
using System;
using System.Collections.Generic;

public static class FishDataBase
{
	public static List<FishData> AllFishes = new List<FishData>()
	{
		new FishData("Atlantic Cod", "Cod", 40f, 1.25f, "Brown", FishRarity.Common ,"Gregarious"),
		new FishData("Atlantic Salmon", "Salmon", 0.80f, 2f, "Grey", FishRarity.Common ,"Calm"),
		new FishData("Golden Carp", "Carp", 45f, 2.3f, "Golden",  FishRarity.Common,"Rapid"),
		new FishData("Blue Catfish", "Catfish", 40f, 1.4f, "Blue",  FishRarity.Common,"Quiet"),
		new FishData("Clownfish", "Clownfish", 0.2f, 0.008f, "Orange and white",  FishRarity.Common,"Friendly"),
		new FishData("Pond Bream", "Bream", 0.6f, 1.0f, "Silver", FishRarity.Common, "Calm"),
		new FishData("Green Perch", "Perch", 1.1f, 1.3f, "Green", FishRarity.Common, "Friendly"),
		new FishData("Marsh Minnow", "Minnow", 0.1f, 0.4f, "Light Green", FishRarity.Common, "Wild"),
		new FishData("Bass", "Bass", 60f, 5.1f, "Dark Gray", FishRarity.Rare, "Aggressive"),
		new FishData("Atlantic Sturgeon", "Surgeon", 140f, 2.0f, "Light Brown", FishRarity.Rare, "Sage"),
		new FishData("Trout", "Trout", 55f, 3.0f, "Rainbow", FishRarity.Rare, "Calm"),
		new FishData("Storm Barracuda", "Barracuda", 5.5f, 2.8f, "Steel Blue", FishRarity.Rare, "Aggressive"),
		new FishData("Elder Carp", "Elder Carp", 4.2f, 2.2f, "Golden", FishRarity.Rare, "Sage"),
		new FishData("Spotted Pike", "Pike", 3.5f, 2.5f, "Dark Green", FishRarity.Uncommon, "Rapid"),
		new FishData("Azure Snapper", "Snapper", 2.0f, 1.8f, "Blue", FishRarity.Uncommon, "Wild"),
		new FishData("Copper Sunfish", "Sunfish", 1.4f, 1.5f, "Copper", FishRarity.Uncommon, "Gregarious"),
		new FishData("Atlantic Mahi Mahi", "Mahimahi", 40.0f, 2.0f, "", FishRarity.Uncommon ,"Calm"),
		new FishData("Atlantic Mahi Mahi", "Mahimahi", 40.0f, 2.0f, "", FishRarity.Uncommon ,"Wild"),
		new FishData("Red Tuna", "Tuna", 60f, 2f, "Blue", FishRarity.Epic, "Strong"),
		new FishData("Crimson Marlin", "Marlin", 45f, 7f, "Red", FishRarity.Epic, "Furious"),
		new FishData("The Leviathan", "Unknown", 3000f, 15.0f, "Black", FishRarity.Legendary ,"Furious"),
		new FishData("The Storm Serpent", "Serpent", 900f, 12f, "Dark Blue", FishRarity.Legendary, "Strong"),
		new FishData("The Wise Example", "Wise Example", 900f, 12f, "Dark Blue", FishRarity.Legendary, "Sage"),
	};
	
	
	private static Random rng = new Random();
	
	private static Dictionary<FishRarity, float> rarityChances = new Dictionary<FishRarity, float>()
	{
		{ FishRarity.Common, 0.55f},
		{FishRarity.Uncommon, 0.25f},
		{FishRarity.Rare, 0.12f},
		{FishRarity.Epic, 0.06f},
		{FishRarity.Legendary, 0.02f}
	};

	public static FishData GetRandomFishByRarity()
	{
		double roll = rng.NextDouble(); 
		float cumulative = 0f;
		FishRarity chosenRarity = FishRarity.Common;
		
		foreach(var kv in rarityChances)
		{
			cumulative += kv.Value;
			if(roll < cumulative)
			{
				chosenRarity = kv.Key;
				break;
			}
		}
		
		var candidates = AllFishes.FindAll(f => f.Rarity == chosenRarity);
		if(candidates.Count == 0)
		{
			return AllFishes[0];
		} 
		string location = PlayerData.Instance.CurrentLocation;
		GD.Print(location);
		switch (location)
		{
			case "Lilie's" :
				//Calm and Friendly
				candidates = candidates.FindAll(f => (f.Temperament == "Calm" || f.Temperament == "Quiet" || f.Temperament == "Sage" || f.Temperament == "Gregarious" || f.Temperament == "Friendly"));
				break;
			case "Pakko Beach" :
				//Normal
				break;
			case "Lake" :
				//To add 🤷‍♂️🤷‍♂️🤷‍♂️🤷‍♂️🤷‍♂️🤷‍♂️🤷‍♂️🤷‍♂️
				break;
			case "River" :
				//Rapide
				candidates = candidates.FindAll(f => (f.Temperament == "Rapid" || f.Temperament == "Wild" || f.Temperament == "Strong" || f.Temperament == "Furious" || f.Temperament == "Agressive"));
				break;
			default :
				break;
			
		}
		return candidates[rng.Next(candidates.Count)];
	}
}


public static class FishDifficulty
{
	public static int GetMashGoal(FishRarity rarity)
	{
		switch ( rarity)
		{
			case FishRarity.Common : return 10;
			case FishRarity.Uncommon : return 12;
			case FishRarity.Rare : return 15;
			case FishRarity.Epic : return 20;
			case FishRarity.Legendary : return 25;
			default : return 15;
		}
	}
	
	public static float GetMashTime(FishRarity rarity)
	{
		switch ( rarity)
		{
			case FishRarity.Common : return 6f;
			case FishRarity.Uncommon : return 5f;
			case FishRarity.Rare : return 4f;
			case FishRarity.Epic : return 3.5f;
			case FishRarity.Legendary : return 3f;
			default : return 6f;
		}
	}
	public static (float min, float max) GetWaitingTimeRange(FishRarity rarity)
	{
		switch ( rarity)
		{
			case FishRarity.Common : return (1.5f, 3f);
			case FishRarity.Uncommon : return (2f, 4f);
			case FishRarity.Rare : return (3.5f, 5f);
			case FishRarity.Epic : return (4.5f, 6f);
			case FishRarity.Legendary : return (5f, 8f);
			default : return (2f, 5f);
		}
	}
}
