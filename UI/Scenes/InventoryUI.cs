using Godot;
using System;


public partial class InventoryUI : Control
{
	Inventory inventory; //This resource should be shared with character
	GridContainer InventoryGrid;
	public override void _Ready()
	{
		Events.Instance.InventoryChanged += UpdateInventory;
		InventoryGrid = GetNodeOrNull<GridContainer>("HBoxContainer/VBoxContainer2/TabContainer/MarginContainer/Inventory");
		Visible = false;
		GD.PrintErr("InventoryUI instance path: ", GetPath());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Inventory")) {
			bool is_hidden = !Visible;	
			if (is_hidden) 
			{
				Events.Instance.EmitSignal(Events.SignalName.ChangeIsInteracting, true);
				Show();
			}
			else {

				Events.Instance.EmitSignal(Events.SignalName.ChangeIsInteracting, false);
				Hide();
			}
		}

		if(Visible) 
		{
			QueueRedraw();
		}
	}

	public void SetInventory(Inventory inventory) {
		GD.PrintErr("Debug InventoryUI: Setting Inventory...");
		this.inventory = inventory;
		GD.PrintErr("Debug InventoryUI: Done setting Inventory...");
		
	}

	public void UpdateInventory() 
	{	
		GD.PrintErr("Debug InventoryUI -- UpdateInventory Called");
		if (inventory == null)
		{
			GD.PrintErr("Error from InventoryUI: Inventory returned null!");
		}

		for (int i = 0; i < inventory.InventorySpace.Count ; i++) 
		{
			Godot.Collections.Array<InventoryItem> array = inventory.InventorySpace[i];

			for (int j = 0; j < array.Count; j++)
			{
				InventoryItem inv_item = array[j];

				Node ratio_cont = InventoryGrid.GetChild(j*i+j);
				TextureRect texture_rect= ratio_cont.GetNodeOrNull<TextureRect>("TextureRect");

				
				if(inv_item != null)
				{
					GD.PrintErr("Debug InventoryUI: Assigning Texture of item");
					texture_rect.Texture = inv_item.Texture;
					texture_rect.DrawTexture(texture_rect.Texture, Vector2.Zero);
					GD.PrintErr(texture_rect.Texture.ToString());
					GD.PrintErr($"Debug: InventoryGrid Visible: {InventoryGrid.Visible}");
				}
				else
				{
					texture_rect.Texture = null;	//if there are problem, this is probably it. I am unfamilar with this method. 
				}
			}
		}
	}
}
