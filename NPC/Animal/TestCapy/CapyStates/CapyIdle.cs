using Godot;
using System;
using System.Numerics;

public partial class CapyIdle : NPCState<Capybara>
{
    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _Ready()
    {
        base._Ready();
		
    }
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
		SceneTreeTimer Test = GetTree().CreateTimer(5.0f);
		Test.Timeout += () => EmitSignal(SignalName.Finished, WALK);

		if (NPC.AniTree != null) {			//this feels like a very clumsy way of doing this. Prevents us from throwing an error the first time we Enter() here but parent node isn't done w/ ready() yet;
			NPC.AniTree.Set("parameters/conditions/isIdle", true);
			GD.Print ("Anitree isIdle set to TRUE");
		}
    }
    public override void Exit()
    {
        NPC.AniTree.Set("parameters/conditions/isIdle", false);
		GD.Print ("Anitree isIdle set to FALSE");
    }
}
