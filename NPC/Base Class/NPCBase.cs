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

	public virtual string InteractSceneString {get; set;} = "";         //Currently, capybara has a SkinningScene var that esentially replaces this. Depending on where the interact features and maybe even dialouge implementation go, this might be what we want to use in the future?

    public void DamageHealth(float Damage) 
	{
		Health -= Damage; 
		if (Health <= 0 && !isDead) {
			Death();
		}
	}

	virtual public void Death() //Note this is called Death but signal is DeathSignal
	{
		isDead = true;
		EmitSignal(SignalName.DeathSignal);
	}

	virtual public void Interact() 
	{

	}

	public void Rotate(Godot.Vector3 direction) //GAME-BREAKING GLITCH: This no longer adjusts 
	{
		//GD.PrintErr("----------------------------------------------------------------------------------------");
		//GD.PrintErr("Rotating gay ass lil bitch");
		//GD.PrintErr($"Direction Passed in: {direction}");
		Transform3D transform = Transform;
		Basis a = Transform.Basis;			

		//GD.PrintErr($"a -- my basis: {a}");

		Basis b =  Basis.LookingAt(-direction);

		//GD.PrintErr($"b -- basis I wanna look at : {b}");


		Godot.Quaternion aQuat = a.GetRotationQuaternion();
		Godot.Quaternion bQuat = b.GetRotationQuaternion();
		aQuat = aQuat.Normalized();
		bQuat = bQuat.Normalized();

		Godot.Quaternion interpolatedQuat = aQuat.Slerp(bQuat, .5f);

		//GD.PrintErr($"interpolatedQuat -- {interpolatedQuat}");

		//snapping after total distance <.01. Can adjust later. Not sure if this is the best way to handle this -- what if turnRate doesn't allow for a sufficiently close margin. Might be able to check if last interpolatedQuat is = current interpolatedQuat. Meaning that the slerp has hit max change. 
		
		if (aQuat.IsEqualApprox(bQuat)) {		
			transform.Basis = new Basis(bQuat);
			Transform = transform;
			return;
		}
		//GD.PrintErr($"My Old Transform.Basis -- {Transform.Basis}");

		transform.Basis = new Basis(interpolatedQuat);
		Transform  = transform;
		//GD.PrintErr($"My New Transform.Basis -- {Transform.Basis}");
		///GD.PrintErr($"Delta between this basis and where I want to be: Delta X: {b.X - Transform.Basis.X} Delta Y: {b.Y - Transform.Basis.Y} Delta Z: {b.Z - Transform.Basis.Z}"); 
	}
	

}
