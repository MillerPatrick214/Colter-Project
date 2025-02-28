using Godot;
using System;

[GlobalClass]
public partial class InteractPickUpLogic : InteractLogic
{

	public void Interact(Item3D item) 
	{
		Events.Instance.EmitSignal(Events.SignalName.PickUp, item.ItemResource);
		item.QueueFree();
	}
}
