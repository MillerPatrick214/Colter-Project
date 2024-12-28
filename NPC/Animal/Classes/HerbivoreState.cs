using Godot;
using System;

public partial class CapyState : State
{

	//this should probably be the child node of a generic NPC state node
	public const String IDLE = "CapyIdle";	//Will update as behavioral states become more clear. Might also need to include an eat state or something 
	public const String WALK = "CapyWalk";	//Needs to walk with purpose to a "Food" node of some sort
	public const String ALERT = "CapyAlert";//Pause turn towards stimuli to assess threat
	public const String FLEE = "CapyFlee"; //Bolt away from threat
	public const String FALL = "NPCFall";	
	
	public override void _Ready()
	{
				 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
