using Godot;
using System;

public partial class GunCam : Camera3D
{
	// Called when the node enters the scene tree for the first time.

	[Export]
	public NodePath CameraPath;

	Camera3D camera;
	public override void _Ready()
	{
		camera = GetNodeOrNull<Camera3D>(CameraPath);
		if (camera == null)
		{
			GD.PrintErr("GunCam ERROR: unable to connect to camera");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GlobalTransform = camera.GlobalTransform;
	}
}
