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
		 if (fish !=null && fish.Image!=null){
			_fishIcon.Texture = fish.Image;
			_fishIcon.Visible = true;
		}
		else{
			_fishIcon.Texture = null;
			_fishIcon.Visible = false;
		}
	}

	public override Variant _GetDragData(Vector2 atPosition)
	{
		if (_currentFish == null) return default;

		// Aperçu visuel
		var preview = new TextureRect();
		preview.Texture = _fishIcon.Texture;
		preview.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
		preview.Size = new Vector2(64, 64);
		SetDragPreview(preview);

		return this; // On envoie le slot lui-même
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		return data.Obj is InventorySlot;
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		var sourceSlot = data.Obj as InventorySlot;
		if (sourceSlot == null) return;

		// Échanger les données dans la liste globale
		var fishList = PlayerData.Instance.CaughtFishes;
		FishData temp = fishList[this._index];
		fishList[this._index] = fishList[sourceSlot._index];
		fishList[sourceSlot._index] = temp;

		// Rafraîchir tout l'inventaire
		GetNode<Inventory>("res://Inventory.tscn").UpdateDisplay();
	}
}
