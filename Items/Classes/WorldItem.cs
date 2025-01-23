using Godot;
using System;

public partial class WorldItem : RigidBody3D //base for all items and tools visible in the game world
{

	[Export]
	public bool IsInteractable = true;
	[Export]
	public Resource ItemResource {get; set;}

	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	virtual public void Interact() {
		
	}
}
