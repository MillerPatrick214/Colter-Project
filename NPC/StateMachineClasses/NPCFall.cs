using Godot;
using System;

public partial class NPCFall<T> : NPCState<T> where T : NPCBase
{	

	Vector3 FallVelocity = Vector3.Zero; 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		FallVelocity.Y = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void PhysicsUpdate(double delta)
	{
		FallVelocity.Y -= (float)(delta * gravity);
		NPC.Velocity += FallVelocity;

		if (NPC.IsOnFloor()) {
			EmitSignal(SignalName.Finished, IDLE);
		}
	}
	
}
