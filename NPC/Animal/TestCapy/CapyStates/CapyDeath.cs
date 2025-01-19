using Godot;
using System;

public partial class CapyDeath : NPCState<Capybara>
{
	// Called when the node enters the scene tree for the first time.

	Vector3 threatPosition;
	public override void _Ready()
	{
		base._Ready();
		NPC.DeathSignal += () => EmitSignal(SignalName.Finished, DEATH);
		
	}

    public override void Enter(string previousStatePath)
    {
		NPC.AniTree.Set("parameters/conditions/isDead", true);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void PhysicsUpdate(double delta)
	{
		NPC.Velocity = new Vector3(0,0,0);

	}

}
