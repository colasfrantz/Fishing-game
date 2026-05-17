using Godot;

public partial class InventorySlot : PanelContainer
{
	private TextureRect _fishIcon;
	private int _index;

	public void Display(FishData fish, int index)
	{
		_index = index;
		
		// SÉCURITÉ : On cherche le nœud ICI s'il est null
		if (_fishIcon == null) {
			_fishIcon = GetNodeOrNull<TextureRect>("FishIcon");
		}

		if (_fishIcon == null) {
			// Si on arrive ici, c'est que ce n'est pas une instance de InventorySlot.tscn
			return; 
		}

		if (fish != null && fish.Image != null) {
			_fishIcon.Texture = fish.Image;
			_fishIcon.Visible = true;
			GD.Print($"Slot {index} : Affiche {fish.Name}");
		} else {
			_fishIcon.Texture = null;
			_fishIcon.Visible = false;
		}
	}
	

}
