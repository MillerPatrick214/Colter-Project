using Godot;
using System;

public partial class InteractRayCast : RayCast3D
{
	Node LastSeen = null;

	public override void _Ready()
	{
		CollisionMask = 2;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{	
		
		Node InteractableObject = (Node)GetCollider();
		
		GD.Print($"InteractableObject Type: {InteractableObject?.GetType()}");

		if ((InteractableObject is NPCBase|| InteractableObject is WorldItem || InteractableObject is null ) && InteractableObject != LastSeen) { 

			Events.Instance.EmitSignal(Events.SignalName.PlayerRayCast, InteractableObject)

			}

		LastSeen = InteractableObject;

		if (Input.IsActionJustPressed("InteractWorld") && InteractableObject != null) {
			if (InteractableObject is WorldItem worldItem && worldItem.IsInteractable) {
				GD.Print("Recognized Object as WorldItem");
				worldItem.Interact();
			}
			
			else if (InteractableObject is NPCBase NPC && NPC.IsInteractable) {
				GD.Print("Recognized Object as NPCBase");
				NPC.Interact();
			}
		}
	}
}

