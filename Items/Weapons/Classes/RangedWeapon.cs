using Godot;
using System;

public partial class RangedWeapon : Weapon
{
	//goal is to not need to override hardly anything for children classes
	[Export]
	public virtual float ProjectileVelocity { get; set; }  = 0f;
	
	public virtual string AmmoPath { get; set;} = ""; 
	bool CanFire;
	bool IsAiming;		//Player is aiming
	bool IsInteracting; //Player is interacting

	PackedScene AmmoScene;
	AnimationPlayer AniPlayer;
	AnimationTree AniTree;
	Marker3D WeaponEnd;
	Timer timer;

	/*						//Not including as this is base class as we will likely have bows etc that this class should inherit form
	GpuParticles3D Smoke;
	OmniLight3D Flash;
	*/

	public override void _Ready()
	{
		CanFire = true;
		IsAiming = false;
		IsInteracting = false;


		AniTree = GetNodeOrNull<AnimationTree>("AnimationTree");
		AmmoScene = ResourceLoader.Load<PackedScene>(AmmoPath);	//AmmoPath NEEDS to be specificed in each child instance;
		WeaponEnd = GetNodeOrNull<Marker3D>("WeaponEnd");
		timer = GetNodeOrNull<Timer>("Timer");

		timer.Timeout += CanFireReset;
		Events.Instance.ChangeIsInteracting += (interactbool) => IsInteracting = interactbool;
	}

	public override void _Process(double delta)
	{
		if (!IsInteracting) {
			if (Input.IsActionPressed("Aim")) {
				Aim(true, delta);
			}

			else {
				Aim(false, delta);
			}

			if(IsAiming && Input.IsActionJustPressed("UseItem")) {
				Fire();
			}
		}

	}

	public void CanFireReset() {	//will be removed after reloading is completed
		CanFire = true;
	}

	public void Aim(bool aimBool, double delta){					//Tis will almost definitely need to be re-worked as animation improves
		IsAiming = aimBool;
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

	}


	public void LaunchProjectile() {
		RigidBody3D ProjectileInstance = AmmoScene.Instantiate<RigidBody3D>();
		GetTree().Root.AddChild(ProjectileInstance);
		ProjectileInstance.GlobalPosition = WeaponEnd.GlobalPosition;
		ProjectileInstance.LinearVelocity = WeaponEnd.GlobalTransform.Basis.Z.Normalized() * ProjectileVelocity;
	}
}
