using Godot;
using System;

public partial class InteractRayCast : RayCast3D
{
	[Signal]
	public delegate void InteractableSeenEventHandler(GodotObject InteractableObject);

	[Signal]
	public delegate void InteractableLostEventHandler();

	public override void _Ready()
	{
		CollisionMask = 2;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{	
		GodotObject InteractableObject = GetCollider();
		if (InteractableObject != null) {
			EmitSignal(SignalName.InteractableSeen, InteractableObject);
			GD.Print("Emitting Seen Signal");
		}

		else if (InteractableObject == null) {
			EmitSignal(SignalName.InteractableLost);
		}
	}
}
