using Godot;
using System;


public partial class InventoryUI : Control
{
	public static Inventory inventory; //This resource should be shared with character
	public override void _Ready()
	{
		Visible = false;
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

	public void SetInventory(ref Inventory inventory) {
		//GD.PrintErr("Debug InventoryUI: Setting Inventory...");
		InventoryUI.inventory = inventory;

		//GD.PrintErr("Debug InventoryUI: Done setting Inventory...");
		
	}
}
