using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public partial class CapyAlert : NPCState<Capybara>
{
	Godot.Vector3 lastKnownPosition;
    public override void _Ready()
    {
        base._Ready();
		NPC.Sensed += ()=> EmitSignal(SignalName.Finished, "Alert");
	}

    public override void PhysicsUpdate(double delta)
	{
		NPC.Velocity = Godot.Vector3.Zero;		//I hate doing this and would much rather fix the fucking Walk state than constantly set velocity to zero in these states in the future
		if (NPC.GetFocused() != null)
		{
			GD.Print($"Current Focus = {NPC.GetFocused().Name}");
			
			AssessThreat();
		}
	}

	public void AssessThreat() {
		Godot.Vector3 NPClocation = NPC.GlobalPosition;
		Godot.Vector3 TargetLocation = NPC.GetFocused().GlobalPosition;
		Godot.Vector3 direction = TargetLocation - NPClocation;

		if (!NPC.isInVisionCone())
		{											//if you can't see the location of the "noise"  keep turning
			direction = direction.Normalized(); 	//normalized for rotation -- not sure we need to do this necessarily. 
			NPC.Rotate(direction);
		}
		
		
		else 										// if you can see the location of the "noise", try moving the raycast to confirm whether it's a threat
		{														
			NPC.setRayCast(direction);
			GodotObject collObj = NPC.GetRayCollision();

			if (collObj.IsClass("Character")) {
				GD.Print("Detected you bobber kurwva!!!!!!!!");
			}

			else {
				GD.Print("Attempting to locate cause of noise");
			}
		}
	}
}
