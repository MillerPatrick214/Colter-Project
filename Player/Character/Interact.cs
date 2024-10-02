using Godot;
using System;

public partial class Interact : Node
{
	// Called when the node enters the scene tree for the first time.

	CharacterBody3D char3D;
	InteractRayCast RayCast;
	public override void _Ready()
	{
		RayCast = GetNodeOrNull<InteractRayCast>("%InteractRayCast");

		if (RayCast == null) {
			GD.Print("Interact Node: RayCast Returned Null");
		}
		
		else {
			GD.Print("Interact Node: Raycast returned successfully");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void PickUp(Node Object) {

	}
}
