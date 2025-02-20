using Godot;
using System;

public partial class HitBoxComponent : Area3D
{
	[Export]					//Using Area3d for hitboxes. Collisionbodies are for physics calcs.
	public HealthComponent HealthComponent;

	public void Damage(float damage)
	{
		if (HealthComponent != null)
		{
			HealthComponent.Damage(damage);
		}

		else
		{
			GD.PrintErr($"{GetPathTo(this)}, error! no healthcomponent found.");
		}
	}
}
