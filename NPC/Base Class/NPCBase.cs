using Godot;
using System;

public partial class NPCBase : CharacterBody3D
{	
	[Signal]
	public delegate void DeathSignalEventHandler();
	[Export]
	float Health = 100.0f;
	[Export]
	public float Speed = 5.0f;
	[Export]
	public float JumpVelocity = 4.5f;
	[Export]
	public bool isDead = false;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	[Export]
	public bool IsInteractable = false;	

	public virtual string InteractSceneString {get; set;} = "";			//Currently, capybara has a SkinningScene var that esentially replaces this. Depending on where the interact features and maybe even dialouge implementation go, this might be what we want to use in the future?
	
	public override void _Ready() 
	{
		
	}

	public override void _PhysicsProcess(double delta)
	{
		
		MoveAndSlide();

		if (Health <= 0 && !isDead) {
			Death();
		}
	}

	public void DamageHealth(float Damage) 
	{
		Health -= Damage; 
	}

	virtual public void Death() //Note this is called Death but signal is DeathSignal
	{
		isDead = true;
		EmitSignal(SignalName.DeathSignal);
	}

	virtual public void Interact() 
	{

	}

	public void Rotate(Godot.Vector3 direction) //this has been copied over from CapyWalk -- need to figure out the inheritance on this function and others 
	{
		Transform3D transform = Transform;

		Basis a = Transform.Basis;
		Basis b = Basis.LookingAt(-direction);

		Godot.Quaternion aQuat = a.GetRotationQuaternion();
		Godot.Quaternion bQuat = b.GetRotationQuaternion();
		aQuat = aQuat.Normalized();
		bQuat = bQuat.Normalized();

		Godot.Quaternion interpolatedQuat = aQuat.Slerp(bQuat, .1f);

		//snapping after total distance <.01. Can adjust later. Not sure if this is the best way to handle this -- what if turnRate doesn't allow for a sufficiently close margin. Might be able to check if last interpolatedQuat is = current interpolatedQuat. Meaning that the slerp has hit max change. 
		
		if (aQuat.IsEqualApprox(bQuat)) {		
			transform.Basis = new Basis(bQuat);
			Transform = transform;
			return;
		}

		transform.Basis = new Basis(interpolatedQuat);
		Transform  = transform;
	}
	

}
