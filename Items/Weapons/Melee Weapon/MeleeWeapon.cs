using Godot;
using System;

public partial class MeleeWeapon : Weapon
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public float StrikeDamage = 0f;
	[Export]
	public Area3D Hurtbox;
	CollisionShape3D CollShape;
	AnimationPlayer AniPlayer;
	public override void _Ready()
	{
		if (Hurtbox != null)
		{
			Hurtbox.AreaEntered += Hit;
		}
		AniPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void Attack()
	{
		AniPlayer.Play("SWING");
	}

	public void Hit(Area3D area)
	{
		if (area is HitBoxComponent hit_box)
		{
			hit_box.Damage(StrikeDamage);
			GD.Print($"{Name} attacked {hit_box.GetParent().Name} for {StrikeDamage} damage!");
		}
	}
}
