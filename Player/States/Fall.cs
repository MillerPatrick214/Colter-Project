using Godot;
using System;

public partial class Fall : PlayerState
{
	// Called when the node enters the scene tree for the first time.
	Vector3 FallVelocity = Vector3.Zero; 

	public override void Enter(String previousState)
	{
		FallVelocity.Y = 0; 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void PhysicsUpdate(double delta)
	{
		FallVelocity.Y -= (float)(gravity * delta); //casting var as a float

		player.Velocity += FallVelocity;
		
		if (player.IsOnFloor()){
			EmitSignal(SignalName.Finished, IDLE);
		}	

		player.MoveAndSlide();
	}
}
