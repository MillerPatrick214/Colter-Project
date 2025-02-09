using Godot;
using System;

public partial class InvSlotUI : SlotUI
{

	int row;
	int column;

	public override void _Ready()
	{
		base._Ready();
		if (SlotID == -1) 
		{
			GD.PrintErr("InvSlotUI: Error! Slot still set to default of -1. Will not function");
		}
		else 
		{
			SlotID = Mathf.Clamp(SlotID, 0, 11);
		}
	}

	public override void _Process(double delta)
	{
		if (Visible)
		{
			if (is_mouse_on && MouseHolding != null && currItem == null)
			{
				InventoryItemUI itemUI = MouseHolding;
				if (Input.IsActionJustReleased("UseItem"))
				{
					MouseHolding = null;
					try
					{
						Player.Instance.Inventory.MoveItem(itemUI.item, row, column);
					}

					catch (ArgumentException e)
					{
						GD.PrintErr($"Inventory.MoveItem: {e}");
						return;
					}

					itemUI.QueueFree();
					Events.Instance.EmitSignal(Events.SignalName.InventoryChanged);
				}
			}
		}
	}

	public override void UpdateSlot()
	{
		int inventory_columns = Player.Instance.Inventory.GetColumns();
		row = SlotID / inventory_columns;
		column = SlotID % inventory_columns;

		InventoryItem newItem = Player.Instance.Inventory.InventorySpace[row][column];

		if (currItem != newItem)
		{
			currItem = newItem;

			if (currItem != null)
			{
				CreateChild(currItem);
			}
		} 
	}
}
