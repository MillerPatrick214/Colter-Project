using Godot;
using System;

public partial class Character : CharacterBody3D
{

    public override void _Ready()
    {
		
    }
	
    [Export]
	public float Speed = 4f;

	[Export]
	public float SprintSpeed = 7f;

	[Export]
	public float JumpImpulse = 100f;

}



