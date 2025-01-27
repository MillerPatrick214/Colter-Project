using Godot;
using System;

public partial class UI : Control
{
	// Called when the node enters the scene tree for the first time.

	public enum UIState		//instead of setting show/hide manually we'll do it here.
	{
		Skinning,
		Reloading,
		Dialogue,
		Pause,
		Inventory,
	}

	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	
	public override void _Process(double delta)
	{

	}

}
