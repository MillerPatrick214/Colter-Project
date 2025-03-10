using Godot;

public partial class HealthComponent : Node
{
	[Signal]
	public delegate void DeathSignalEventHandler();
	[Export]
	float MaxHealth;
	float health;
	public override void _Ready()
	{
		health = MaxHealth;
	}

	public void Damage(float damage)
	{
		health -= damage;
		if (health <= 0)
		{
			EmitSignal(SignalName.DeathSignal);
		}
	}

	public float GetHealth()
	{
		return health;
	}


}
