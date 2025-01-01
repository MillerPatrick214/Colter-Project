using Godot;
using System;
using System.Numerics;
using System.Threading;

public partial class Capybara : NPCBase
{
	
	[Signal]
	public delegate void SensedEventHandler();

	// Called when the node enters the scene tree for the first time.
	PackedScene SkinningScene;

	Node3D FocusedBody = null;
	//List<Node3D> SensedBodies;
	Area3D HearingArea;
	Area3D VisionCone;
	RayCast3D VisionRay; 

	AnimationPlayer AniPlayer; 

	//so flow is
	//	1. Added to sensed
	//	2. Rotate VisualArea to include body
	//	3. Adjust RayCast to attempt to find body
	//	4.		If : RayCast finds body, set to flee (and then somehow pass data over to it telling it the direction to flee in? Maybe use nav )
	//	5. 		Else : RayCast Can't find body after x seconds, go back to Idle

	[Export]
	float WalkSpeed = 5f;
	[Export]
	float FleeSpeed = 15f;

	public override void _Ready()
	{
		base._Ready();
		SkinningScene = GD.Load<PackedScene>("res://Skinning/DeerSkinTEST.tscn"); // load scene

		HearingArea = GetNodeOrNull<Area3D>("HearingArea"); 
		VisionCone = GetNodeOrNull<Area3D>("VisionCone");
		VisionRay = GetNodeOrNull<RayCast3D>("VisionRay");
		AniPlayer = GetNodeOrNull<AnimationPlayer>("capybara test walking EXPORT/AnimationPlayer");

		HearingArea.BodyEntered += (Node3D body) => SensedAdd(body);		
		HearingArea.BodyExited += (Node3D body) => SensedRemove(body);

		VisionCone.BodyEntered += (Node3D body) => SensedAdd(body);
		VisionCone.BodyExited += (Node3D body) => SensedRemove(body);
		
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Velocity != Godot.Vector3.Zero) {
			AniPlayer.Play("ArmatureAction_001");
		}

		else {
			AniPlayer.Stop();
		}

	}

	public override void Death() {
		IsInteractable = true;
	}

	public override void Interact() {
		if (SkinningScene == null) {
			GD.Print("Capybara: Error, skinning scene came back as null");
		}
		Events.Instance.EmitSignal(Events.SignalName.BeginSkinning, SkinningScene);
	}

	public float GetWalkSpeed() {
		return WalkSpeed;
	}

	public Node3D GetFocused() {
		return FocusedBody;
	}

	public bool isInVisionCone() {
		return VisionCone.OverlapsBody(FocusedBody);
	}

	public void setRayCast(Godot.Vector3 direction) {
		VisionRay.TargetPosition = direction;
	}

	public GodotObject GetRayCollision() 
		{
		GodotObject collObj = VisionRay.GetCollider(); 
		return collObj;
		}
	
	
	public void SensedAdd(Node3D body) {
		//if (!SensedBodies.Any()) {					//This should catch any body that enters despite which state we're in and change the mode to Alert\
			GD.Print($"Body that was detected is of {body.GetType()}");
			if (body.IsClass("Character")) { 
				GD.Print("Successfully Detected Character");
				EmitSignal(SignalName.Sensed);
				FocusedBody = body;					//FIXME -- currently this will just add the last body as the focused body if that makes sense idk. We should probably draw from list based off of distance of sound/sight in the future?
			}
		//}

		/*
		if (!SensedBodies.Contains(body)) {
			SensedBodies.Add(body);
			GD.Print($"{Owner.Name} -- Added {body} to list of sensed bodies");
		}
		*/

	}

	public void SensedRemove(Node3D body) {
		/*
		if (SensedBodies.Contains(body)) {
			SensedBodies.Remove(body);
			GD.Print($"{Owner.Name} -- removed {body} from list of sensed bodies");
		}
		*/

		FocusedBody = null;
	}
}

