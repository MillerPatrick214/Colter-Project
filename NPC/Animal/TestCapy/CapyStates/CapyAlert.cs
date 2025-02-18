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

    public override void Enter(string previousStatePath)
    {
		NPC.Velocity = Godot.Vector3.Zero;
		NPC.MoveAndSlide();
	}

    public override void PhysicsUpdate(double delta)
	{
		if (NPC.GetThreat() != null)
		{
			GD.Print($"Current Focus = {NPC.GetThreat().Name}");
			
			AssessThreat();
		}

		

		if (Susometer >= 100) {
			EmitSignal(SignalName.Finished, FLEE);
		}

		if (!NPC.IsOnFloor()) {
				EmitSignal(SignalName.Finished, FALL);
			}
	}

	public void AssessThreat()
	{
		if (NPC == null)
		{
			GD.PrintErr("NPC is null in AssessThreat!");
			return;
		}

		var focused = NPC.GetThreat();
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
		GD.Print($"VisionConeCheck: {NPC.isInVisionCone()}");
		
		if (NPC.isInVisionCone())
		{

			if (collObj != null && collObj.IsClass("CharacterBody3D"))			
			{
				CharacterBody3D CharacterNode = collObj as CharacterBody3D;
				
				if (CharacterNode.IsInGroup("ThreatLevel3")) {	//ThreatLevel3 bs needs revision. 
					GD.Print("Detected you bobber kurwva!!!!!!!!");
					Susometer += 2;
				}

				else
				{
					GD.Print("Attempting to locate cause of noise");
				}
			}
			
		}
	}

    public override void Exit()
    {
        base.Exit();
		Susometer = 0;
    }
}