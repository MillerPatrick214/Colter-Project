using Godot;
using System;


public partial class InventoryUI : Control
{
	Inventory inventory; //This resource should be shared with character
	GridContainer InventoryGrid;
	public override void _Ready()
	{
		Events.Instance.InventoryChanged += UpdateInventory;
		InventoryGrid = GetNodeOrNull<GridContainer>("HBoxContainer/VBoxContainer2/MarginContainer/InvGrid");
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
			GD.PrintErr($"For i loop, inventory.InventorySpace.Count returned: {inventory.InventorySpace.Count}");
			Godot.Collections.Array<InventoryItem> array = inventory.InventorySpace[i];
			GD.PrintErr($"InventoryUI!: Entered For loop i{i}!");
			LogInventoryGridChildren();
			for (int j = 0; j < array.Count; j++)
			{
				GD.PrintErr($"For i loop, array.Count returned: {array.Count}");
				GD.PrintErr($"InventoryUI!: Entered For loop j{j}!");
				InventoryItem inv_item = array[j];

				Node inv_rect = InventoryGrid.GetChild(j*i+j);


				if (inv_rect == null)
				{
					GD.PrintErr("InventoryUI: InvRect Returned null");
				}

				if(inv_item != null && inv_rect.GetClass() == "TextureRect"  )
				{
					GD.PrintErr("Debug InventoryUI: Assigning Texture of item");
					TextureRect texture_rect = (TextureRect)inv_rect; 
					texture_rect.Texture  = inv_item.Texture;
					GD.PrintErr($"Debug InventoryUI: A texture is Null. InvRect {texture_rect.Texture} --- inv_item {inv_item.Texture} ");
					GD.PrintErr($"Debug: InventoryGrid Visible: {InventoryGrid.Visible}");
					
				}
			}
		}
	}
	public void LogInventoryGridChildren()
	{
		int childCount = InventoryGrid.GetChildCount(); // Get the total number of children
		GD.PrintErr("Total children in InventoryGrid: " + childCount);

		for (int i = 0; i < childCount; i++)
		{
			Node child = InventoryGrid.GetChild(i); // Get the child node at index i
			GD.PrintErr("Child at index " + i + ": " + child.Name); // Log the name or other properties of the child
		}
	}
}
