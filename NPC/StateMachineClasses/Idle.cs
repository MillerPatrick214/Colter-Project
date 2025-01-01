using Godot;
using System;

public partial class NPCIdle<T> : NPCState<T> where T : NPCBase 
{
	// Called when the node enters the scene tree for the first time.

	Vector3 ZeroVect = Vector3.Zero;

	public override void _Ready()
	{
		NPC.Velocity = ZeroVect; 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		
	}
}
