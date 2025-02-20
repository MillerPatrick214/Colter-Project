using Godot;
using System;

public partial class CapyDeath : NPCState<Capybara>
{
	// Called when the node enters the scene tree for the first time.

	Vector3 threatPosition;
	public override void _Ready()
	{
		base._Ready();
		NPC.GetNode<HealthComponent>("HealthComponent").DeathSignal += () => EmitSignal(SignalName.Finished, DEATH);
		
	}

    public override void Enter(string previousStatePath)
    {
		NPC.AniTree.Set("parameters/conditions/isDead", true);
		NPC.Velocity = Godot.Vector3.Zero;
    }
}
