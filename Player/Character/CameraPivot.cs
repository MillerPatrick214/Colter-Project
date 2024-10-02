using Godot;
using System;

public partial class Marker3D : Godot.Marker3D
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("whatever", GetPath());
	}

	public override void _Process(double delta)
	{

	}
} 

