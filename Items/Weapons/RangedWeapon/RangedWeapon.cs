using Godot;
using System;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public abstract partial class RangedWeapon : Weapon
{
	//goal is to not need to override hardly anything for children classes

	//will need to add scene info for reloading stuff

	[Export]
	public virtual Vector3 ADSPosition {get; set;} = Vector3.Zero;
	[Export]
	public virtual Vector3 ADSRotation {get; set;} = Vector3.Zero;
	[Export]
	public virtual Vector3 DefaultPosition {get; set;} = Vector3.Zero;
	[Export]
	public virtual Vector3 DefaultRotation{get; set;} = Vector3.Zero;
	public abstract float ProjectileVelocity { get; set; }
	[Export]
	public virtual float SwayFactor {get; set;}


	//Recoil Values
	/*
	[Export]
	public Curve RecoilRotationX;
	[Export]
	public Curve RecoilRotationZ;
	[Export]
	public Curve RecoilPositionZ;
	[Export]
	public Vector3 RecoilAmplitude = Vector3.One;
	[Export]
	public float LerpSpeed = 1.0f;
	Vector3 TargetRotation;
	Vector3 TargetPosition;
	public float CurrentTime;
	*/
	
	
	public abstract string AmmoPath { get; set;}
	bool CanFire;
	bool IsAiming;		//Player is aiming
	bool IsInteracting; //Player is interacting
	float ADS_Speed;
	PackedScene AmmoScene;
	AnimationPlayer AniPlayer;
	AnimationTree AniTree;
	Marker3D WeaponEnd;
	Timer timer;
	public override void _Ready()
	{
		base._Ready();
		//TargetRotation.Y = Rotation.Y;
		//CurrentTime = 1;

		CanFire = true;
		IsAiming = false;
		IsInteracting = false;

		AniTree = GetNodeOrNull<AnimationTree>("AnimationTree");

		if (AniTree == null) {
			GD.PrintErr("RangedWeapon: Unable to find AniTree");
		}

		AniPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");

		if (AniPlayer == null) {
			GD.PrintErr("RangedWeapon: Unable to find AniPlayer");
		}

		AmmoScene = ResourceLoader.Load<PackedScene>(AmmoPath);	//AmmoPath NEEDS to be specificed in each child instance;
		if (AmmoScene == null) {
			GD.PrintErr("RangedWeapon: Unable to find AmmoScene");
		}
		WeaponEnd = GetNodeOrNull<Marker3D>("WeaponEnd");
		if (WeaponEnd == null) {
			GD.PrintErr("RangedWeapon: Unable to find WeaponEnd");
		}

		timer = GetNodeOrNull<Timer>("Timer");
		if (timer == null) {
			GD.PrintErr("RangedWeapon: Unable to find timer");
		}

		timer.Timeout += CanFireReset;
		Events.Instance.ChangeIsInteracting += (interactbool) => IsInteracting = interactbool;
	}
	/*
	public void ApplyRecoil()
	{
		RandomNumberGenerator rand = new();
		RecoilAmplitude.Y *= Mathf.Abs(rand.Randi()) % 2 > 0 ? 1 : -1; //rand flip to amiplitude creates more natural looking recoil? we'll have to see
		TargetRotation.X = RecoilRotationX.Sample(0);
		TargetRotation.Z = RecoilRotationZ.Sample(0);
		TargetPosition.Z = RecoilPositionZ.Sample(0);
		CurrentTime = 0;
	}
	*/

	public override void _Process(double delta)
	{
		Aim(delta);
	}
	/*
    public override void _PhysicsProcess(double delta)
    {
        if (CurrentTime < 0)
		{
			CurrentTime += (float)delta; 
			float pos_z = Mathf.Lerp(Position.Z, TargetPosition.Z, LerpSpeed * (float)delta);
			float rot_z = Mathf.Lerp(Rotation.Z, TargetRotation.Z, LerpSpeed * (float)delta);	
			float rot_x = Mathf.Lerp(Rotation.X, TargetRotation.X, LerpSpeed * (float)delta);

			Position = new Vector3(Position.X, Position.Y, pos_z);
			Rotation = new Vector3(rot_x, Rotation.Y, rot_z);

			TargetRotation.Z = RecoilRotationZ.Sample(CurrentTime) * RecoilAmplitude.Y;
			TargetRotation.X = RecoilRotationX.Sample(CurrentTime) * -RecoilAmplitude.X;
			TargetPosition.Z = RecoilPositionZ.Sample(CurrentTime) * RecoilAmplitude.Z;
		}
    }
	*/

    public void SetIsAiming(bool tf)
	{
		IsAiming = tf;

	}

	public void CanFireReset() {	//will be removed after reloading is completed
		CanFire = true;
	}

	public void Aim( double delta){					//Tis will almost definitely need to be re-worked as animation improves
		var current_animation = AniTree.Get("anim_player/current_animation");
		GD.PrintErr("current_animation " + current_animation.ToString());
		float currentAimState = (float)AniTree.Get("parameters/Blend2/blend_amount"); 
		if (IsAiming) {
			float newAimState = Mathf.Lerp(currentAimState, 1, (float)(5 * delta));
			AniTree.Set("parameters/Blend2/blend_amount", newAimState);
		}
		else {
			
			
			
				float newAimState = Mathf.Lerp(currentAimState, 0, (float)(5 * delta));
				AniTree.Set("parameters/Blend2/blend_amount", newAimState);
			
		}
	}

	public void Fire() {
		AniTree.Set("parameters/OneShot/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);	
		LaunchProjectile();
		//ApplyRecoil();
	}

	public virtual void ResetEmitters() {
		
	}


	public void LaunchProjectile() {
		RigidBody3D ProjectileInstance = AmmoScene.Instantiate<RigidBody3D>();
		GetTree().Root.AddChild(ProjectileInstance);
		ProjectileInstance.GlobalPosition = WeaponEnd.GlobalPosition;
		ProjectileInstance.ApplyCentralImpulse(-WeaponEnd.GlobalTransform.Basis.Z.Normalized() * ProjectileVelocity);
	}
}
