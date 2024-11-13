using Godot;
using System;

public partial class TestRoom : Node3D
{
	[Export]
	float DebugTimeScale = 1;		//standard = 1

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Engine.TimeScale = DebugTimeScale;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}


}
