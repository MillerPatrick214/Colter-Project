using Godot;
using System;


public partial class EquipSlotUI : SlotUI 
{
	// Called when the node enters the scene tree for the first time.
	
	[Export]
	public Equippable.EquipSlot slot_type = Equippable.EquipSlot.NOTSET;

	public override void _Ready()
	{
		base._Ready();
		if (slot_type == Equippable.EquipSlot.NOTSET)
		{
			GD.PrintErr("EquipSlotUI: slot_type = Equippable.EquipSlot.NOTSET");
		}
		if (SlotID == -1) 
		{
			GD.PrintErr("InvSlotUI: Error! Slot still set to default of -1. Will not function");
		}
		else if (SlotID > 12 || SlotID < 0)
		{
			GD.PrintErr($"InvSlotUI: Error! Slot outside of range. Will not function. Slot ID:{SlotID}");
		}
		else 
		{
			SlotID = Mathf.Clamp(SlotID, 0, 12);
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
						PlayerInventory.player_inv.MoveItem(itemUI.item, SlotID);
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
		InventoryItem newItem = PlayerInventory.player_inv.EquippedSlotList[SlotID].ItemInSlot;
		/*
		GD.PrintErr($"{SlotID} -- New Item: {newItem}");
		GD.PrintErr($"{SlotID} -- PlayerInventory.player_inv.EquippedSlotList[SlotID].ItemInSlot: {PlayerInventory.player_inv.EquippedSlotList[SlotID].ItemInSlot}");
		GD.PrintErr("----------------------------------------------------------------------------------------------------");
		*/

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
