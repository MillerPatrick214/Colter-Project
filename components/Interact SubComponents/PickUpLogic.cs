using Godot;
using System;

[GlobalClass]
public partial class PickUpLogic : InteractLogic
{
	public void Interact(Node ParentNode) 
	{
		if (ParentNode is not Item3D) {GD.PrintErr($"Error InteractLogic {this.ResourcePath}. Parent isn't Item3D");}

		Item3D item = (Item3D)ParentNode;
		Events.Instance.EmitSignal(Events.SignalName.PickUp, item.ItemResource);
		item.QueueFree();
		
	}
}
