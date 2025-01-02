using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public partial class CapyAlert : NPCState<Capybara>
{	

	float Susometer = 0;					// Want to use accumulater
	Godot.Vector3 lastKnownPosition;
    public override void _Ready()
    {
        base._Ready();
		NPC.Sensed += ()=> EmitSignal(SignalName.Finished, ALERT); 
	}

    public override void PhysicsUpdate(double delta)
	{
		NPC.Velocity = Godot.Vector3.Zero;		//I hate doing this and would much rather fix the fucking Walk state than constantly set velocity to zero in these states in the future
		if (NPC.GetFocused() != null)
		{
			GD.Print($"Current Focus = {NPC.GetFocused().Name}");
			
			AssessThreat();
		}

		

		if (Susometer == 100) {
			EmitSignal(SignalName.Finished, FLEE);
		}
	}

	public void AssessThreat()
	{
		if (NPC == null)
		{
			GD.PrintErr("NPC is null in AssessThreat!");
			return;
		}

		var focused = NPC.GetFocused();
		if (focused == null)
		{
			GD.PrintErr("GetFocused() returned null!");
			return;
		}

		if (focused.GlobalPosition == Godot.Vector3.Zero)
		{
			GD.PrintErr("Focused object's GlobalPosition is null!");
			return;
		}

		Godot.Vector3 NPClocation = NPC.GlobalPosition;
		Godot.Vector3 TargetLocation = focused.GlobalPosition;
		Godot.Vector3 direction = TargetLocation - NPClocation;
		GD.Print("Direction: ", direction);

		NPC.setRayCast(direction);
		GodotObject collObj = NPC.GetRayCollision();
		
		direction.Y = 0;
		direction = direction.Normalized();
		
		NPC.Rotate(direction);
		
		if (NPC.isInVisionCone())
		{

			if (collObj != null)			
			{
				if (collObj.IsClass("Character"))
				{
					GD.Print("Detected you bobber kurwva!!!!!!!!");
					Susometer += 1;
				}
				else
				{
					GD.Print("Attempting to locate cause of noise");
				}
			}
		}
	}
}