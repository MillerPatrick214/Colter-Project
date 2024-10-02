using Godot;
using System;

public partial class AnimatedSprite3D : Godot.AnimatedSprite3D
{
	public override void _Ready()
	{
		
		this.Play("Back Camera");
	}


	public override void _Process(double delta)
	{
		
		
	}
}
