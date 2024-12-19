using Godot;
using System;

public partial class MeleeWeapon : Weapon
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	float StrikeDamage = 0f;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
