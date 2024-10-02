using Godot;
using System;

public partial class Walk : PlayerState
{
	// Called when the node enters the scene tree for the first time.
	public override void Enter(String previousState) {

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void PhysicsUpdate(double delta)
	{
		
		Vector3 direction = Vector3.Zero;
		Vector3 targetVelocity = Vector3.Zero;
		Vector3 forward = player.GlobalTransform.Basis.Z.Normalized(); //foward direction? This shit is confusing me honestly. The reason we need to do this is so we're always moving relative to the camera 
		Vector3 right = player.GlobalTransform.Basis.X.Normalized(); //Right? 
		
		if (Input.IsActionPressed("Left")) {
			direction -= right;
		}
		if (Input.IsActionPressed("Right")) {
			direction += right;
		}
		if (Input.IsActionPressed("Forward")) {
			direction -= forward;
			//animation.Play("Back Camera"); 
			
		}
		if (Input.IsActionPressed("Back")) {
			direction += forward;
			//animation.Play("Front Camera");
		}
		
		if (direction != Vector3.Zero) {
			direction = direction.Normalized();
		}
		
		if (Input.IsActionPressed("Sprint")) { 
			targetVelocity = direction * player.SprintSpeed;
		}
		else {
			targetVelocity = direction * player.Speed;
		}
		player.Velocity = targetVelocity;
		player.MoveAndSlide();	
	

		if (!player.IsOnFloor()) {
		EmitSignal(SignalName.Finished, FALL);
		}

		if(direction == Vector3.Zero) {
			EmitSignal(SignalName.Finished, IDLE);
		}

	}
}
