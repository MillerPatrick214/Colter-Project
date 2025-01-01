using Godot;
using System;

public partial class NPCState<T> : State where T : NPCBase //using template to allow for greater flexibility
{

	protected T NPC;
	protected NavigationAgent3D NavAgent;
	public const string FALL = "Fall";
	public const string WALK = "Walk";
	public const string IDLE = "Idle";
	public const string ALERT = "Alert";
	
	
	//so here we will continue to add string defs for different states as needed
	
	public override void _Ready()
	{

		NPC = Owner as T;

		NavAgent = NPC.GetNodeOrNull<NavigationAgent3D>("NavigationAgent3D"); 

		if (NPC == null)
        {
            GD.PrintErr($"NPCState: Owner is not of type {typeof(T).Name}.");
        }
		
	}

}
