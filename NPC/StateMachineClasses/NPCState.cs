using Godot;
using System;
using System.Threading.Tasks;

public partial class NPCState<T> : State where T : NPCBase //using template to allow for greater flexibility
{

	protected T NPC;
	public const string FALL = "Fall";
	public const string WALK = "Walk";
	public const string IDLE = "Idle";
	public const string ALERT = "Alert";
	public const string FLEE = "Flee";
	
	
	//so here we will continue to add string defs for different states as needed
	
	public override async void _Ready()
	{

		NPC = Owner as T;
		//GD.Print("Awaiting NPC Owner to be Ready");

		await ToSignal(NPC, SignalName.Ready);
		
		//GD.Print("NPC Ready!!!");

		if (NPC == null)
        {
            GD.PrintErr($"NPCState: Owner is not of type {typeof(T).Name}.");

        }
	}
}
