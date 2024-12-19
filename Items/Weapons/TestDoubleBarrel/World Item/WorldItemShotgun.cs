using Godot;
using System;

public partial class WorldItemShotgun : WorldItem
{
	// Called when the node enters the scene tree for the first time.

	PackedScene ItemUsable;

	public override void _Ready()
	{
		ItemUsable = GD.Load<PackedScene>("res://Items/Weapons/TestDoubleBarrel/TestDoubleBarrel.tscn");
		IsInteractable = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public override void Interact() {
		Events.Instance.EmitSignal(Events.SignalName.PickUp, ItemUsable);
		QueueFree();

	}
}
