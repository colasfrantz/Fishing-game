using Godot;

public partial class InventorySlot : PanelContainer
{
	private TextureRect _fishIcon;
	public int Index;
	public FishData CurrentFish;

	public override void _Ready()
	{
		_fishIcon = GetNode<TextureRect>("FishIcon");
		
		// Pour le contour : assure-toi que ton PanelContainer a un StyleBoxFlat
		// avec une bordure (Border Width) définie dans l'inspecteur.
	}

	public void Display(FishData fish, int index)
	{
		Index = index;
		CurrentFish = fish;
		
		if (fish != null && fish.Image != null)
		{
			_fishIcon.Texture = fish.Image;
			_fishIcon.Visible = true;
		}
		else
		{
			_fishIcon.Texture = null;
			_fishIcon.Visible = false;
		}
	}

	// --- LOGIQUE DRAG AND DROP ---
	public override Variant _GetDragData(Vector2 atPosition)
	{
		if (CurrentFish == null) return default;

		// Aperçu visuel pendant qu'on déplace le poisson
		var preview = new TextureRect();
		preview.Texture = _fishIcon.Texture;
		preview.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
		preview.Size = new Vector2(64, 64);
		SetDragPreview(preview);

		return this; // On s'envoie soi-même comme donnée
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		return data.Obj is InventorySlot;
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		var sourceSlot = data.Obj as InventorySlot;
		if (sourceSlot == null) return;

		var caughtFishes = PlayerData.Instance.CaughtFishes;

		// On vérifie que les deux index existent dans la liste
		if (this.Index < caughtFishes.Count && sourceSlot.Index < caughtFishes.Count)
		{
			// Échange des données dans PlayerData
			FishData temp = caughtFishes[this.Index];
			caughtFishes[this.Index] = caughtFishes[sourceSlot.Index];
			caughtFishes[sourceSlot.Index] = temp;

			// Rafraîchir l'affichage (on remonte au parent Inventory)
			GetTree().CurrentScene.FindChild("Inventory", true, false).Call("DisplayInventory");
		}
	}
}
