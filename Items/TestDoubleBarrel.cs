using Godot;
using System;
using System.Numerics;
using System.Xml.Schema;

public partial class TestDoubleBarrel : Node3D
{
	// Called when the node enters the scene tree for the first time.
	[Signal]
	public delegate void FireEventHandler();
	CharacterBody3D Char;
	AnimationPlayer AniPlayer;
	CamPivot CamPivotNode;
	Timer timer;

	Marker3D  BarrelMarker;

	//AnimatedSprite3D Sprite;

	bool AimingState;
	bool CanFire;

	PackedScene LeadBall;

	AudioStreamPlayer GunEffect;

	
	public override void _Ready()
	
	{		
		GD.Randomize();	//called to randomize seed for shotgun pattern


		LeadBall = ResourceLoader.Load<PackedScene>("res://lead_ball.tscn");
		
		AimingState = false;
		CanFire = true;

		Char = GetTree().GetNodesInGroup("PlayerCharacter")[0] as CharacterBody3D;

		GunEffect = GetNodeOrNull<AudioStreamPlayer>("Gunshot");

		AniPlayer = GetNodeOrNull<AnimationPlayer>("Barrel End/AnimationPlayer");

		CamPivotNode = Char.GetNodeOrNull<CamPivot>("CamPivot");

		timer = GetNodeOrNull<Timer>("Timer");
		timer.Timeout += ReloadTimerReset;
		
		BarrelMarker = GetNodeOrNull<Marker3D>("Barrel End");				// SO for some reason I can't access the AnimatedSprite3D "bang" node at all. I'll be hiding the marker instead.

		 
		
		if (Char == null || AniPlayer == null || CamPivotNode == null || BarrelMarker == null || timer == null) {
			GD.Print("TestDoubleBarrel: Error, some or all nodes returned null.");
			GD.Print(Char != null ? "Char Found" : "Char null");
			GD.Print(AniPlayer != null ? "AniPlayer Found" : "AniPlayer null");
			GD.Print(CamPivotNode != null ? "CamPivotNode found" : "CamPivotNode null");
			GD.Print(BarrelMarker != null ? "BarrelMarker found" : "BarrelMarker null");
			GD.Print(timer != null ? "timer found" : "timer null");
		}


		BarrelMarker.Hide();
		CamPivotNode.AimSignal += Aiming;		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("UseItem") && AimingState && CanFire) {
			Bang();
			
		}

	}

	public void Aiming(bool isAiming) 
	{
		AimingState = isAiming;
	}

	public void Bang() {
		ShootBall();
		EmitSignal(SignalName.Fire);
		BarrelMarker.Show(); 
		GunEffect.Play();
		timer.Start(5);
		CanFire = false;
		AniPlayer.Play("Bang");
		AniPlayer.AnimationFinished += (Godot.StringName oldName) => BarrelMarker.Hide();		//first use of the lamba function for signals here.

	}

	public void ReloadTimerReset() {
		CanFire = true;
	}

	public void ShootBall() {					//I really need to read up on vector math. Even tho I'm doing lin algebra rn basis make no fucking sense to me. 
		for ( int i = 0; i < 9; i++) {			//note this is particular to the shotgun as it will be firing 8 bullets.
		RigidBody3D BallInstance = LeadBall.Instantiate<RigidBody3D>(); //moved this inside the loop, need to instantiate each time -- duh.
		GetTree().Root.AddChild(BallInstance);

		BallInstance.GlobalPosition = BarrelMarker.GlobalPosition;
		//BallInstance.GlobalTransform = BarrelMarker.GlobalTransform; I don't think we need this line? I might for arrows as they have to be lined up with the front of the weapon.

		BallInstance.LinearVelocity = -RotateVector(BarrelMarker.GlobalTransform.Basis).Z * 50.0f;	// multiplying speed by the forward basis of the BarrelMarker (-)

		}
		 
	}

	public Godot.Basis RotateVector(Godot.Basis zBasis) {		// I think this is going to work but this represents how shitty I am at vector math. 
		Basis basedist = zBasis;

		float randX = (float)GD.Randfn(0, .07);
		float randY = (float)GD.Randfn(0, .07);
		float randZ = (float)GD.Randfn(0, .07);

		basedist.Z += new Godot.Vector3(randX, randY, randZ);



		/* float randRot = (float)GD.Randfn(0, .15);
		GD.Print($"Debug: {randRot}");
		basedist.X = basedist.X.Rotated(new Godot.Vector3(1, 0, 0), randRot);

		randRot = (float)GD.Randfn(0, .15);
		GD.Print($"Debug: {randRot}");

		basedist.Y = basedist.Y.Rotated(new Godot.Vector3(0, 1, 0), randRot);

		randRot = (float)GD.Randfn(0, .15);
		GD.Print($"Debug: {randRot}");

		basedist.Z = basedist.Z.Rotated(new Godot.Vector3(0, 0, 1), randRot); */

		return basedist;



		
	}

}
