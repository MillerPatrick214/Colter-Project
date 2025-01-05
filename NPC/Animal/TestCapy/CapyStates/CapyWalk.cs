using Godot;
using System;
using System.Numerics;

public partial class CapyWalk : NPCState<Capybara>
{	
	bool enterComplete = false;


	public override void _Ready()
	{
		base._Ready();
		NavAgent.NavigationFinished += NavFinished;
		
	}

	public override void Enter(string previousStatePath)
    {
		NPC.AniTree.Set("parameters/conditions/isWalking", true);
		GD.Print ("Anitree isWalking set to TRUE");

		Godot.Vector3 newRandLocation = new Godot.Vector3(0,0,0);
		Random rnd = new Random();

		newRandLocation.X = rnd.Next(-100, 100) + NPC.GlobalPosition.X;
		newRandLocation.Z = rnd.Next(-100, 100) + NPC.GlobalPosition.Z;

		GD.Print("Before bounds check: ", newRandLocation);

		newRandLocation = NavigationServer3D.MapGetClosestPoint(NavAgent.GetNavigationMap(), newRandLocation);

		GD.Print("After bounds check: ", newRandLocation);

		NavAgent.TargetPosition = newRandLocation;
		//NPC.setAnimation("CapybaraAnimations/CapyWalk");
		enterComplete = true;
		
	}

	public override void PhysicsUpdate(double delta)
	{
		if (enterComplete) {												//Very cheesy for now. I think I need to bake flags into base classes or better yet send signals up to state machine for exit/enter to ensure animations are complete etc.
			Godot.Vector3 destination = NavAgent.GetNextPathPosition();
			Godot.Vector3 LocalDestination = destination - NPC.GlobalPosition;
			Godot.Vector3 direction = LocalDestination.Normalized();
			direction.Y = 0;									
			NPC.Rotate(direction);

			
			NPC.Velocity = direction * NPC.GetWalkSpeed();

			NPC.MoveAndSlide();
		
			if (!NPC.IsOnFloor()) {
				EmitSignal(SignalName.Finished, FALL);
			}
		}
	}

	public void NavFinished() 
	{
		GD.Print("Navigation finished");
		EmitSignal(SignalName.Finished, IDLE);
	}

    public override void Exit()
    {
		NPC.Velocity = Godot.Vector3.Zero; // Reset velocity to zero
    	NPC.MoveAndSlide(); // Apply the reset velocity
		//GD.Print("Exiting CapyWalk state. Velocity reset to: ", NPC.Velocity);
		NPC.AniTree.Set("parameters/conditions/isWalking", false);
		GD.Print ("Anitree isWalking set to FALSE");
		enterComplete = false;
    }
	

	public bool IsApproxEqualCustom(Godot.Vector3 front, Godot.Vector3 target, float tolerance) 	//not currently using. Custom checker as IsApproxEqual() wasn't working well.
	{
		return front.DistanceTo(target) <= tolerance; 
	}
}
