using Godot;
using System;

public partial class HitBoxComponent : Area3D
{
	[Signal] public delegate void TrappedEventHandler(bool tf);
	[Export]public HealthComponent HealthComponent;
	

	public void Damage(float damage)
	{
		if (HealthComponent != null)
		{
			HealthComponent.Damage(damage);
		}

		else
		{
			GD.PrintErr($"{GetPath()}, error! no healthcomponent found.");
		}
	}

	public void SetTrapped(bool IsImmobilized)
	{
		EmitSignal(SignalName.Trapped, IsImmobilized);

	}
}
