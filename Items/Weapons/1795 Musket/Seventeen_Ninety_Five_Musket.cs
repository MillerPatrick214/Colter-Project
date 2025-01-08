using Godot;
using System;

public partial class Seventeen_Ninety_Five_Musket : RangedWeapon
{
	public override float ProjectileVelocity { get; set; }= 1000f;
	public override string AmmoPath { get; set; }= "res://lead_ball.tscn";


}
