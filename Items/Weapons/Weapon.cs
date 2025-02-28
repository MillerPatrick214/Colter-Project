using Godot;
using System;

public partial class Weapon : Item3D
{
	[Export]
	public virtual Vector3 DefaultPlayerPosition {get; set;} = Vector3.Zero;
	[Export]
	public virtual Vector3 DefaultPlayerRotation{get; set;} = Vector3.Zero;
	[Export]
	public virtual Vector3 DefaultNPCPosition {get; set;} = Vector3.Zero;
	[Export]
	public virtual Vector3 DefaultNPCRotation {get; set;} = Vector3.Zero;
	bool isInteracting = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		
		if (GetParent().Name == "PlayerItemMarker")
		{
			Position = DefaultPlayerPosition;
			RotationDegrees = DefaultPlayerRotation;
		}

		if (GetParent().Name == "NPCItemMarker")
		{
			GD.PrintErr($"Weapon {this.GetPath()} has Owner of NPC Base -- setting position of {DefaultNPCPosition}, Rotation as {DefaultNPCRotation}");
			Position = DefaultNPCPosition;
			RotationDegrees = DefaultNPCRotation;
		}

		Events.Instance.ChangeIsInteracting += (InteractBoolean) => isInteracting = InteractBoolean;
	}

}
