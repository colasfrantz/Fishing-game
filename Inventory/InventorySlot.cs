using Godot;
using System;

public partial class InventorySlot : TextureRect
{
	private FishData _fish;
	private int _index;

	// Cette fonction sera appelée par l'inventaire pour configurer le slot
	public void Setup(FishData fish, int index)
	{
		_fish = fish;
		_index = index;

		// 1. Gestion de l'image
		if (_fish.Image != null)
		{
			Texture = _fish.Image;
		}
		else 
		{
			// Placeholder si pas d'image, pour éviter que ce soit invisible
			// Tu peux charger une texture par défaut ici
			 GD.PrintErr($"Pas d'image pour {_fish.Name}");
		}
		
		// 2. Gestion du Tooltip (Nom au survol)
		TooltipText = $"{_fish.Name}\n{_fish.Rarity}\n{_fish.Weight}kg";
		
		// Pour que l'image s'adapte bien au carré
		ExpandMode = ExpandModeEnum.IgnoreSize;
		StretchMode = StretchModeEnum.KeepAspectCentered;
		CustomMinimumSize = new Vector2(64, 64); // Taille de la case
	}

	// --- LOGIQUE DRAG AND DROP ---

	// Qu'est-ce qu'on traîne ?
	public override Variant _GetDragData(Vector2 atPosition)
	{
		// On crée une prévisualisation visuelle qui suit la souris
		var preview = new TextureRect();
		preview.Texture = Texture;
		preview.ExpandMode = ExpandModeEnum.IgnoreSize;
		preview.Size = new Vector2(64, 64);
		preview.Modulate = new Color(1, 1, 1, 0.5f); // Semi-transparent
		
		SetDragPreview(preview);
		
		// On retourne l'objet complet ou ses données. Ici, on retourne le slot lui-même.
		return this;
	}

	// Est-ce qu'on peut déposer quelque chose ici ?
	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		// On accepte seulement si l'objet traîné est un InventorySlot
		return data.Obj is InventorySlot;
	}

	// Que se passe-t-il quand on lâche ?
	public override void _DropData(Vector2 atPosition, Variant data)
	{
		var sourceSlot = data.Obj as InventorySlot;
		
		if (sourceSlot == this) return; // On a reposé sur soi-même

		// Échange visuel des données (Swap)
		FishData tempFish = this._fish;
		this.Setup(sourceSlot._fish, this._index);
		sourceSlot.Setup(tempFish, sourceSlot._index);

		// Note : Idéalement, il faudrait aussi échanger les positions dans
		// la liste PlayerData.Instance.CaughtFishes pour que l'ordre soit sauvegardé.
	}
}
