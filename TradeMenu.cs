using Godot;
using System;

public partial class TradeMenu : Control
{
	Godot.Collections.Array<InventoryItem> PlayerInvItems;
	Godot.Collections.Array<InventoryItem> TradeComponentInvItems;

	[Export]
	Control PlayerTradeContainer;
	[Export]
	Control TradeComponentContainer;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public TradeMenu()
	{

		
	}

	public void PopulateList(Godot.Collections.Array<InventoryItem> list, Inventory inventory)
	{
		list.Clear();
		
		foreach (EquipInvSlot slot in inventory.EquippedSlotList)
		{
			InventoryItem item = slot.ItemInSlot;
			
			if (item == null) { continue; }

			list.Add(item);
		}
		
		foreach (Godot.Collections.Array<InventoryItem> subarray in inventory.InventorySpace)
		{
			foreach (InventoryItem inv_item in subarray)
			{
				InventoryItem item = inv_item;
				if (item == null) { continue; }
				list.Add(item);
			}
		}
		
		//Will need to add clothing once that is implemented
		

	}

	public void PopulateUIContainer(Control TradeContainer)
	{
		
	}
	
}
