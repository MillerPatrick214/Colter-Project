using Godot;
using System;

public partial class BowieKnife : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.PrintErr("BowieKnife INstance: ", GetPath());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
