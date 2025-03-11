using Godot;
using System;

public partial class FeatureDisplayCam : Marker3D
{
	[Export] Node3D FeaturedParent;
	[Export] float PivotSpeed;
	Vector3 curr_rot;
	public override void _Ready()
	{
		curr_rot = GlobalRotation;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GlobalRotation = curr_rot;

		float parent_y_rot = FeaturedParent.GlobalRotation.Y;
		float curr_y = curr_rot.Y;

		curr_y = Mathf.LerpAngle(curr_y, parent_y_rot, PivotSpeed * (float)delta);

		curr_rot = new(curr_rot.X, curr_y, curr_rot.Z);
	}
}
