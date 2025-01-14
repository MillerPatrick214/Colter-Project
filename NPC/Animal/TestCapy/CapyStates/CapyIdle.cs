using Godot;
using System;
using System.Numerics;
using System.Threading;

public partial class CapyIdle : NPCState<Capybara>
{
    // Called every frame. 'delta' is the elapsed time since the previous frame.

	bool isIdleConfirmed; // serves as a test as to whether or not we are still in idle and we should pay attention to the timer;
    public override void _Ready()
    {
        base._Ready();
		
    }

	SceneTreeTimer Test;
    public override void PhysicsUpdate(double delta)
	{
		NPC.Velocity = Godot.Vector3.Zero; // Keep resetting to zero. I hate doing this and would much rather fix the fucking Walk state than constantly set velocity to zero in these states.
    	NPC.MoveAndSlide(); // Apply the reset velocity

		if (!NPC.IsOnFloor()) {
			EmitSignal(SignalName.Finished, FALL);
		}
	}

    public override void Enter(string previousStatePath)
    {
		isIdleConfirmed = true;
		
		if (isIdleConfirmed) { 
			Test = GetTree().CreateTimer(5.0f);
			Test.Timeout += TimeToWalk;
		}


		if (NPC.AniTree != null) {			//this feels like a very clumsy way of doing this. Prevents us from throwing an error the first time we Enter() here but parent node isn't done w/ ready() yet;
			NPC.AniTree.Set("parameters/conditions/isIdle", true);
		}
    }
    public override void Exit()
    {
		Test.Timeout -= TimeToWalk;
		isIdleConfirmed = false; 
        NPC.AniTree.Set("parameters/conditions/isIdle", false);
    }

	public void TimeToWalk() {
		GD.Print("Idle Time Bitch");
		EmitSignal(SignalName.Finished, WALK);
	}
}
