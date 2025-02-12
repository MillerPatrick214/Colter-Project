using Godot;
using System;

public partial class PlayerSingleton : Node
{
	public static PlayerSingleton Instance;
	public static Player player;
	public override void _Ready()
	{
		Instance = this; 
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
}
