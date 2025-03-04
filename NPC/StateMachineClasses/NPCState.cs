using Godot;
using System;

using System.Diagnostics;

public partial class NPCState : State
{

	protected NPCBase NPC;
	public const string FALL = "Fall";
	public const string WALK = "Walk";
	public const string IDLE = "Idle";
	public const string ALERT = "Alert";
	public const string FLEE = "Flee";
	public const string DEATH = "Death"; 
	
	
	public override async void _Ready()
	{
		NPC = Owner as NPCBase;

		await ToSignal(NPC, SignalName.Ready);
	}


}
