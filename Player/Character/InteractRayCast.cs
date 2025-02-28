using Godot;
using Microsoft.VisualBasic;
using System;

public partial class InteractRayCast : RayCast3D
{
	Node LastSeen = null;

	public override void _Ready()
	{
		SetCollisionMaskValue(3, true);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{	
		InteractComponent InteractableObject;
		Node collision = (Node)GetCollider();
		InteractableObject = collision as InteractComponent;
	
		if (InteractableObject != LastSeen) 
		{	
			Events.Instance.EmitSignal(Events.SignalName.PlayerRayCast, InteractableObject);
		}

		LastSeen = InteractableObject;

		if (Input.IsActionJustPressed("InteractWorld") && InteractableObject != null) 
		{
				InteractableObject.Interact();
		}
	}
}

