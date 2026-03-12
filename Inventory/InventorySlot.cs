using Godot;

public partial class InventorySlot : PanelContainer
{
	private TextureRect _fishIcon;
	public int _index;
	public FishData _currentFish;

	public override void _Ready()
	{
		// On récupère le nœud qui affiche l'image
		_fishIcon = GetNode<TextureRect>("FishIcon");
	}

	public void Display(FishData fish, int index)
	{
		_index = index;
		_currentFish = fish;

		if (fish != null && fish.Image != null)
		{
			_fishIcon.Texture = fish.Image;
			_fishIcon.Visible = true;
		}
		else
		{
			_fishIcon.Texture = null;
			_fishIcon.Visible = false; // On cache l'image si la case est vide
		}
	}

	// --- DRAG AND DROP ---

	public override Variant _GetDragData(Vector2 atPosition)
	{
		if (_currentFish == null) return default;

		var preview = new TextureRect();
		preview.Texture = _fishIcon.Texture;
		preview.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
		preview.Size = new Vector2(64, 64);
		SetDragPreview(preview);

		return this;
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		return data.Obj is InventorySlot;
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		var sourceSlot = data.Obj as InventorySlot;
		if (sourceSlot == null) return;

		var fishList = PlayerData.Instance.CaughtFishes;

		// Sécurité pour éviter de crash si on glisse vers un index inexistant
		if (_index < fishList.Count && sourceSlot._index < fishList.Count)
		{
			FishData temp = fishList[_index];
			fishList[_index] = fishList[sourceSlot._index];
			fishList[sourceSlot._index] = temp;

			// On demande à l'inventaire parent de se rafraîchir
			// On cherche le nœud Inventory dans la scène
			GetTree().Root.FindChild("Inventory", true, false).Call("UpdateDisplay");
		}
	}
}
