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
	AnimationPlayer AniPlayer;
	AnimationTree AniTree;
	Marker3D WeaponEnd;
	AudioStreamPlayer3D SoundEffect;
	Timer timer;

	/*						//Not including as this is base class
	GpuParticles3D Smoke;
	OmniLight3D Flash;
	*/

	public override void _Ready()
	{
		CanFire = true;
		IsAiming = false;
		IsInteracting = false;

		AmmoScene = ResourceLoader.Load<PackedScene>(AmmoPath);	//AmmoPath NEEDS to be specificed in each child instance;
		WeaponEnd = GetNodeOrNull<Marker3D>("WeaponEnd");
		SoundEffect = GetNodeOrNull<AudioStreamPlayer3D>("AudioEffect");
		timer = GetNodeOrNull<Timer>("Timer");

		timer.Timeout += CanFireReset;
		Events.Instance.ChangeIsInteracting += (interactbool) => IsInteracting = interactbool;
	}

	public override void _Process(double delta)
	{
		if (!IsInteracting) {
			if (Input.IsActionPressed("Aim")) {
				Aim(true);
			}

			else {
				Aim(false);
			}

			if(IsAiming && Input.IsActionJustPressed("UseItem")) {
				Fire();
			}
		}

	}

	public void CanFireReset() {	//will be removed after reloading is completed
		CanFire = true;
	}

	public void Aim(bool aimBool){					//Tis will almost definitely need to be re-worked as animation improves
		IsAiming = aimBool;
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
		Smoke.Restart();
		*/
		

	}

	public void EmitParticles() {
		WeaponEnd.Show(); 
		//Smoke.Emitting = true;
		AniTree.Set("parameters/OneShot/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
		LaunchProjectile();
		SoundEffect.Play();

		// all the following needs to be replaced w/ reloading
		timer.Start(5);

		//flash
		//sparks
		//smoke
		//etc
		//FixMe
	}



	public void LaunchProjectile() {
		RigidBody3D ProjectileInstance = AmmoScene.Instantiate<RigidBody3D>();
		GetTree().Root.AddChild(ProjectileInstance);
		ProjectileInstance.GlobalPosition = WeaponEnd.GlobalPosition;
		ProjectileInstance.LinearVelocity = WeaponEnd.GlobalTransform.Basis.Z * ProjectileVelocity;
	}
}
