using System;
using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class NPCBase : CharacterBody3D
{	
	
	[Signal] public delegate void SensedEventHandler();
	[Export] public float WalkSpeed = 5.0f;
	[Export] public float RunSpeed = 7.5f;
	[Export] public float JumpVelocity = 4.5f;
	[Export] public bool isDead = false;
	[Export] public NavigationAgent3D NavAgent;
	[Export] public AnimationTree AniTree;
	[Export] public RayCast3D VisionRay; 
	[Export] public Area3D HearingArea;
	[Export] public Area3D VisionCone;
	[Export] public BTPlayer BehaviorTree;
	[Export] public InteractComponent InteractComponent {get; private set;}
	[Export] public HitBoxComponent HitBoxComponent {get; private set;}

	public VisibleOnScreenNotifier3D VisNotifier;
	public bool IsVisibleOnScreen;
	bool isTrapped = false;

	public InteractComponent InteractComponentCasted;
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	[Export]
	public bool IsInteractable = false;	
	public Node3D Focus = null;
	protected BTPlayer BTPlayer; 
	public virtual string InteractSceneString {get; set;} = "";         //Currently, capybara has a SkinningScene var that esentially replaces this. Depending on where the interact features and maybe even dialouge implementation go, this might be what we want to use in the future?

    public override void _Ready()
    {
		UpdateConfigurationWarnings();

		VisNotifier = new();
		AddChild(VisNotifier);

		if (Engine.IsEditorHint())
		{
			return;
		}

		if (InteractComponent != null) 
		{
			InteractComponentCasted = InteractComponent as InteractComponent; 
			InteractComponentCasted.ParentNode = this;
		}

        base._Ready();
		GetNode<HealthComponent>("HealthComponent").DeathSignal += Death;

		HearingArea.BodyEntered += (Node3D body) => SensedAdd(body);		
		VisionCone.BodyEntered += (Node3D body) => SensedAdd(body);

		HearingArea.BodyExited += (Node3D body) => SensedRemove(body);		
		VisionCone.BodyExited += (Node3D body) => SensedRemove(body);

		VisNotifier.ScreenEntered += () => IsVisibleOnScreen = true;
		VisNotifier.ScreenExited += () => IsVisibleOnScreen = false;

		
		BTPlayer = GetNodeOrNull<BTPlayer>("BTPlayer");
		if (BTPlayer == null) {GD.PrintErr($"NPC Base {this.GetPath()}: BTPlayer returned null");}

		HitBoxComponent.Trapped += (tf) => Trapped(tf);
		UpdateConfigurationWarnings();
    }

	public override string[] _GetConfigurationWarnings()
	{
		List<string> warnings = new List<string>();

		if (NavAgent == null){ warnings.Add("Warning: NavigationAgent3D NavAgent Node not set in editor!");}
		if (AniTree == null){ warnings.Add("Warning: AnimationTree AniTree Node not set in editor!");}
		if (VisionRay == null){ warnings.Add("Warning: RayCast3D VisionRay Node not set in editor!");}
		if (HearingArea == null){ warnings.Add("Warning: Area3D HearingArea Node not set in editor!");}
		if (VisionCone == null){ warnings.Add("Warning: Area3D VisionCone Node not set in editor!");}
		if (InteractComponent == null){ warnings.Add("Warning: InteractComponent node not set in editor!");}

		return warnings.ToArray();
	}

	virtual public void Death() //Note this is called Death but signal is DeathSignal
	{
		BTPlayer.Active = false; 
		isDead = true;
		IsInteractable = true;
	}

	virtual public void Interact() 
	{
	}

	public Node3D GetFocus() {
		return Focus;
	}

	public float GetWalkSpeed() {
		return WalkSpeed;
	}

	public float GetRunSpeed() {
		return RunSpeed;
	}

	public void Trapped(bool tf)
	{
		isTrapped = tf;
		BehaviorTree.Blackboard.SetVar("IsTrapped", tf);
		GD.PrintErr("TRAPPED!!!!");
	}


	public void setRayCast(Godot.Vector3 direction) 
	{
		Godot.Vector3 localDir = VisionRay.ToLocal(GlobalPosition + direction);
		VisionRay.TargetPosition = localDir;
	}

	public GodotObject GetRayCollision() 
	{
		GodotObject collObj = VisionRay.GetCollider(); 
		return collObj;
	}


	public void Rotate(Godot.Vector3 direction)
	{
		if (direction == Vector3.Zero) return;
		if (!direction.IsNormalized()) direction = direction.Normalized();

		Transform3D transform = Transform;
		Basis a = Transform.Basis;			

		Basis b =  Basis.LookingAt(-direction);

		Godot.Quaternion aQuat = a.GetRotationQuaternion();
		Godot.Quaternion bQuat = b.GetRotationQuaternion();
		aQuat = aQuat.Normalized();
		bQuat = bQuat.Normalized();

		Godot.Quaternion interpolatedQuat = aQuat.Slerp(bQuat, .5f);
		
		if (aQuat.IsEqualApprox(bQuat)) {		
			transform.Basis = new Basis(bQuat);
			Transform = transform;
			return;
		}

		transform.Basis = new Basis(interpolatedQuat).Orthonormalized();
		Transform  = transform;
	}

	public bool isInVisionCone() 
	{
		return VisionCone.OverlapsBody(Focus);
	}


    public void SensedAdd(Node3D body) 
	{
		//if (!SensedBodies.Any()) {					//This should catch any body that enters despite which state we're in and change the mode to Alert\
		GD.Print($"Body that was detected is of {body.GetClass()}");
		if (body.IsClass("CharacterBody3D") && body.IsInGroup("Human")) 
		{ 
			GD.Print("Successfully Detected Character");
			if (!isDead)						//Temp work around to avoid entering alert from death
			{						
				Focus = body;					//FIXME -- currently this will just add the last body as the focused body if that makes sense idk. We should probably draw from list based off of distance of sound/sight in the future?
				EmitSignal(SignalName.Sensed);
				BTPlayer.Blackboard.SetVar("hasFocus", true);
			}
		}
		
	}

    public override void _Process(double delta)
    {
        base._Process(delta);
		if (IsVisibleOnScreen)
		{
			//ShaderManager.Instance.RegisterOrUpdate(this, GlobalPosition); SAVED FOR SHADERMANAGER UPDATE
		}
    }

	public void OffScreen()
	{
		IsVisibleOnScreen = false;
		//ShaderManager.Instance.OutOfSight(this);		SAVED FOR SHADERMANAGER UPDATE
	}
	public void OnScreen()
	{
		IsVisibleOnScreen = true;
	}

    public override void _PhysicsProcess(double delta)
    {		
		if (Engine.IsEditorHint())
		{
			return;
		}
        if (!IsOnFloor())
		{
			Velocity =  Velocity - new Vector3(0, gravity, 0) * (float)delta;
    	}
		
		MoveAndSlide();
	}

	public void SensedRemove(Node3D body) 
	{

		if ( Focus != null && !HearingArea.OverlapsBody(Focus) && !VisionCone.OverlapsBody(Focus)) //This needs to be totally changed. 
		{
			Focus = null;
			
			BTPlayer.Blackboard.SetVar("hasFocus", false);
		}
	}
}
