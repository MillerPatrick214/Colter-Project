using Godot;
using System;

public partial class RangedWeapon : Weapon
{
	//goal is to not need to override hardly anything for children classes
	[Export]
	float ProjectileVelocity = 100f;
	
	string AmmoPath;
	bool CanFire;
	bool IsAiming;		//Player is aiming
	bool IsInteracting; //Player is interacting
	PackedScene AmmoScene;

	Marker3D WeaponEnd;
	AudioStreamPlayer SoundEffect;
	Timer timer;
	public override void _Ready()
	{
		CanFire = true;
		IsAiming = false;
		IsInteracting = false;
		
		AmmoScene = ResourceLoader.Load<PackedScene>(AmmoPath);
		WeaponEnd = GetNodeOrNull<Marker3D>("WeaponEnd");
		SoundEffect = GetNodeOrNull<AudioStreamPlayer3D>("AudioEffect");
		timer = GetNodeOrNull<Timer>("Timer");

		timer.Timeout += CanFire = true;
		AniTree.AnimationFinished += hideBangSprite;
		Events.Instance.ChangeIsInteracting += (interactbool) => IsInteracting = interactbool;
	}

	public override void _Process(double delta)
	{
		if (!IsInteracting) {
			if (Input.IsActionPressed("Aim")) {
				Aiming(true);
			}

			else {
				Aiming(false);
			}

			if(IsAiming && Input.IsActionJustPressed("UseItem")) {
				Fire();
			}
		}

	}

	public void Aim(bool IsAiming){					//This will almost definitely need to be re-worked as animation improves
		this->IsAiming = IsAiming;
		if (IsAiming) {
			AniTree.Set("parameters/TimeScale/scale", 4);
		}
		else {
			AniTree.Set("parameters/TimeScale/scale", -4);
		}
	}

	public void Fire() {
		/*							Here is what current TestDoubleBarrel has for Fire. I'd like to separate these out into individ functions to allow for easier editing of components with child classes.
		BarrelMarker.Show(); 
		Smoke.Emitting = true;
		AniTree.Set("parameters/OneShot/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
		ShootBall();
		GunEffect.Play();
		timer.Start(5);
		CanFire = false;
		Smoke.Emitting = true;
		Smoke.Restart();
		*/
		

	}

	public void EmitParticles() {
	//FixMe
	}



	public void LaunchProjectile() {
		RigidBody3D ProjectileInstance = AmmoScene.Instantiate<RigidBody3D>();
		GetTree().Root.AddChild(ProjectileInstance);
		ProjectileInstance.GlobalPosition = WeaponEnd.GlobalPosition;
		BallInstance.LinearVelocity = WeaponEnd.GlobalTransform.Basis * ProjectileVelocity;
	}
}
