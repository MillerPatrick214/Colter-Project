using Godot;
using System;

public partial class EightteenThirteenArmyPistol : RangedWeapon
{
	public override string AmmoPath {get; set;} = "uid://dii3es7qdxhn8";
	public override float ProjectileVelocity {get; set;} = 700.0f;
	
	public override Vector3 DefaultPlayerPosition {get; set;} = new(0, -.038f, -0.5f);
	public override Vector3 DefaultPlayerRotation{get; set;} = new(3.0f, 0.0f, 0.0f);

	public override Vector3 DefaultNPCPosition {get; set;} = new(0.005f, 0.029f, -0.078f);
	public override Vector3 DefaultNPCRotation{get; set;} = new(-7.9f, -18.3f, 43.5f);

	public override void _Ready()
	{
		base._Ready();
	}


}
