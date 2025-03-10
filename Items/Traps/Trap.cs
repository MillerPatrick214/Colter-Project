using Godot;
using System;
using System.Threading.Tasks;

public partial class Trap : Item3D
{
	public HitBoxComponent TrappedObject = null;
	AnimationPlayer AniPlayer;
	[Export]Area3D TriggerArea;
	Timer timer;
	[Export] float TriggerDamage; //Damage when trap is initially triggered

	[Export] float DPS = 0;   //Damage dealt every second after trap is triggered

	/// <summary>
	/// How long NPC stays trapped for in seconds. all negative numbers represent indefinitely. If IsImmobilizing is set to false, number will always be 0;
	/// </summary
	[Export] int Duration = -1;	//  allows us to set a default val when it isn't touched in editor

	[Export] bool IsImmobilizing = true;	//Represents whether or not trap immobilizes NPC and holds them in place

	public override void _Ready()
	{
		if (!IsImmobilizing) Duration = 0;
		timer = GetNodeOrNull<Timer>("Timer");
		if (TriggerArea == null) TriggerArea = GetNodeOrNull<Area3D>("TriggerArea");
		AniPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");

		TriggerArea.AreaEntered += (hitbox) => Triggered(hitbox);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public  void Place()
	{

	}


	public async void Triggered(Area3D hitbox)
	{
		GD.PrintErr("Trap detected area!");
		if (hitbox is not HitBoxComponent box || TrappedObject != null) return;
		GD.PrintErr("Trap sprung on hitbox!");
		TrappedObject = box;

		TrappedObject.Damage(TriggerDamage);
		TrappedObject.SetTrapped(IsImmobilizing);

		if (!IsImmobilizing) return;
			
		if (Duration > 0)
		{
			for (int i = 0; i < Duration; ++i)
			{
				await ApplyDPS();
			}

			TrappedObject.SetTrapped(false);
		}

		else
		{
			while (TrappedObject.HealthComponent.GetHealth() > 0)
			{
				await ApplyDPS();
			}
		}
	}
	

	public async Task ApplyDPS()
	{
		timer.Start(1);
		await ToSignal(timer, Timer.SignalName.Timeout);
		TrappedObject.Damage(DPS);
	}

}
