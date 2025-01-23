using Godot;
using System;

public partial class Weapon : Item3D
{
	bool isInteracting = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		Events.Instance.ChangeIsInteracting += (InteractBoolean) => isInteracting = InteractBoolean;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


}
