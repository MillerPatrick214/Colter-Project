using Godot;
using System;
using System.Numerics;

public partial class CapyIdle : NPCState<Capybara>
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.

	
	public override void PhysicsUpdate(double delta)
	{
		NPC.Velocity = Godot.Vector3.Zero; // Keep resetting to zero. I hate doing this and would much rather fix the fucking Walk state than constantly set velocity to zero in these states.
    	NPC.MoveAndSlide(); // Apply the reset velocity
		GD.Print("Velocity: ", NPC.Velocity);

		if (!NPC.IsOnFloor()) {
			EmitSignal(SignalName.Finished, FALL);
		}
	}

    public override void Enter(string previousStatePath)
    {
		
		SceneTreeTimer timer = GetTree().CreateTimer(6.0f);
		timer.Timeout += Test;
		if (NPC.AniPlayer != null) {			//this feels like a very clumsy way of doing this.
			NPC.setAnimation("CapybaraAnimations/CapyHeadDown");
			NPC.setNextAnimation("CapybaraAnimations/CapyHeadDown", "CapybaraAnimations/CapyGraze");
		}
    }

	public override void Exit() {
	}

	public void Test() {
		EmitSignal(SignalName.Finished, WALK);
	}
}
