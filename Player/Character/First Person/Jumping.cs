using Godot;
using System;

public partial class Jumping : PlayerState
{
	// Called when the node enters the scene tree for the first time.

	Vector3 up;

	public override void Enter(String previousState) { 
		
		up = player.GlobalTransform.Basis.Y.Normalized();
		player.Velocity += up * player.JumpImpulse;
		GD.Print("Finished Enter");
		EmitSignal(SignalName.Finished, FALL);
		
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void PhysicsUpdate(double delta)
	{	

	}
}
