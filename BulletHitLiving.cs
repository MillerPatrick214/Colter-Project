using Godot;
using System;

public partial class BulletHitLiving : Decal
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GpuParticles3D particles = GetNodeOrNull<GpuParticles3D>("GPUParticles3D");
		particles.Emitting = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
