using Godot;
using System;

public partial class PlayerState : State
{
	public const String IDLE = "Idle";
	public const String WALK = "Walk";
	public const String JUMPING = "Jumping";
	public const String FALL = "Fall";



	public Character player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = Owner as Character;		
	}

}

