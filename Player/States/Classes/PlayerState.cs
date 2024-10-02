using Godot;
using System;

public partial class PlayerState : State
{
	public const String IDLE = "Idle";
	public const String WALK = "Walk";
	public const String JUMPING = "Jumping";
	public const String FALL = "Fall";

	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();


	public Character player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = Owner as Character;		
	}

	/*public async void OnOwnerReady() {
		await ToSignal(Owner, "ready");
		player = Owner as Character;
		if (player == null) { 
			GD.Print("The PlayerState state type must be used only in the player scene. It needs the owner to be a Player node.");
		} 

	} */
}

