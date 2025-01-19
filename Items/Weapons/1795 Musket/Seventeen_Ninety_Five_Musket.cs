using Godot;
using System;

public partial class Seventeen_Ninety_Five_Musket : RangedWeapon
{
	public override float ProjectileVelocity { get; set; }= 1000f;
	public override string AmmoPath { get; set; }= "res://Items/Weapons/1795 Musket/Projectile/1795LeadBall.tscn";


}
