using Godot;
using System;

public partial class NPCIdle : NPCState
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
		if (!NPC.IsOnFloor()) {
			EmitSignal(SignalName.Finished, FALL);
		}
	}

    public override void Enter(string previousStatePath)
    {
		if (isIdleConfirmed) { 
			Test = GetTree().CreateTimer(5.0f);
			Test.Timeout += TimeToWalk;
		}
		
		NPC.Velocity = Vector3.Zero;
		NPC.MoveAndSlide();

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
		if (Test != null)
		{
			Test.Timeout -= TimeToWalk;
		}

		isIdleConfirmed = false; 
        NPC.AniTree.Set("parameters/conditions/isIdle", false);
    }

	public void TimeToWalk() {
		EmitSignal(SignalName.Finished, WALK);
	}
}
