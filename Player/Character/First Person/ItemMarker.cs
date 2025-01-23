using Godot;
using System;

public partial class ItemMarker : Marker3D
{
	// Called when the node enters the scene tree for the first time.

	public override void _Ready()
	{
		Events.Instance.PickUp += (item) => PickUp(item);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void PickUp(InventoryItem item) {
		var instance = ResourceLoader.Load<PackedScene>(item.ScenePath).Instantiate();
		AddChild(instance);
		var child = GetNodeOrNull<Item3D>(instance.GetPath());
		if (child == null) {GD.PrintErr("ItemMarker -- child returned null on pickup. Can't set as held. ");}
		child.SetHeld(true);
	}
}
