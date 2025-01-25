using Godot;
using System;

public partial class SlotUI : Control
{
	[Export]
	public virtual int SlotID { get; set; } = -1;		//0 - 11 for inv slots & Equip slots
	public static InventoryItemUI MouseHolding;
	protected bool is_mouse_on = false;
	public InventoryItem currItem = null;

	public override void _Ready()
	{
		MouseDefaultCursorShape = CursorShape.PointingHand;
		Events.Instance.InventoryChanged += UpdateSlot;
		ZIndex = 2;		
		Events.Instance.MouseHolding += (item) => MouseHolding = item;
		MouseEntered += () => is_mouse_on = true;
		MouseExited += () => is_mouse_on = false;
		//I think we might want to localize the mouse events here in this node as opposed to the singlgeton if possible. For future not now (1/25/25)
	}

	public virtual void UpdateSlot()
	{
		
	}
	
	public void CreateChild(InventoryItem newItem)
	{
		currItem = newItem;
		var instance = InventoryItemUI.DefaultScene.Instantiate();
		AddChild(instance);
		InventoryItemUI item = GetChild<InventoryItemUI>(0);
		item.SetUpitem(currItem);
	}
}
