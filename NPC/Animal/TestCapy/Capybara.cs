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
	string currState = ""; 
	Node3D Threat = null;		// Holds whatever is curr threat for the NPC. 
	
	//
	//I think we will likely want a curr List of sensed bodies and a body for the "Threat" that is set by the alert mode once "proof" of that threat is visually confirmed or susometer reaches max?
	//

	//List<Node3D> SensedBodies;
	Area3D HearingArea;
	Area3D VisionCone;
	RayCast3D VisionRay; 
	public AnimationTree AniTree;
	public NavigationAgent3D NavAgent;

	//so current flow is
	//	1. Added to sensed
	//	2. Rotate VisualArea to include body
	//	3. Adjust RayCast to attempt to find body
	//	4.		If : RayCast finds body, set to flee (and then somehow pass data over to it telling it the direction to flee in? Maybe use nav )
	//	5. 		Else : RayCast Can't find body after x seconds, go back to Idle

	[Export]
	float WalkSpeed = 5f;
	[Export]
	float FleeSpeed = 7.5f;

	public override void _Ready()
	{
		base._Ready();
		SkinningScene = GD.Load<PackedScene>("res://Skinning/DeerSkinTest.tscn"); // load scene

		HearingArea = GetNodeOrNull<Area3D>("HearingArea"); 
		VisionCone = GetNodeOrNull<Area3D>("VisionCone");
		VisionRay = GetNodeOrNull<RayCast3D>("VisionRay");
		AniTree = GetNodeOrNull<AnimationTree>("AnimationTree");

		NavAgent= GetNodeOrNull<NavigationAgent3D>("NavigationAgent3D"); 

		if (NavAgent == null) {GD.Print("God damn this is fucked! Capybara: NavAgent is null");}
		else {GD.Print("We're Chuned! NavAgent found successfully");} 

		if (AniTree == null) {GD.Print("Capybara: Fuck AniTree is Null");}

		if (HearingArea == null || VisionCone == null || VisionRay == null) {
			 GD.Print((HearingArea == null) ? "Capybara: HearingArea came back as null" : "");
			 GD.Print((VisionCone == null) ? "Capybara: VisionCone came back as null" : "");
			 GD.Print((VisionRay == null) ? "Capybara: VisionRay came back as null" : "");
		}

		HearingArea.BodyEntered += (Node3D body) => SensedAdd(body);		
		//HearingArea.BodyExited += (Node3D body) => SensedRemove(body);

		VisionCone.BodyEntered += (Node3D body) => SensedAdd(body);
		//VisionCone.BodyExited += (Node3D body) => SensedRemove(body);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public override void Death() {
		base.Death(); 
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

	public float GetFleeSpeed() {
		return FleeSpeed;
	}

	public Node3D GetThreat() {
		return Threat;
	}

	public bool isInVisionCone() {
		return VisionCone.OverlapsBody(Threat);
	}


	public void setRayCast(Godot.Vector3 direction) {
		Godot.Vector3 localDir = VisionRay.ToLocal(GlobalPosition + direction);
		VisionRay.TargetPosition = localDir;
	}

	public GodotObject GetRayCollision() 
		{
		GodotObject collObj = VisionRay.GetCollider(); 
		return collObj;
		}
	
	public void SensedAdd(Node3D body) {
		
		//if (!SensedBodies.Any()) {					//This should catch any body that enters despite which state we're in and change the mode to Alert\
		GD.Print($"Body that was detected is of {body.GetClass()}");
		if (body.IsClass("CharacterBody3D") && body.IsInGroup("Human")) 
		{ 
			GD.Print("Successfully Detected Character");
			if (!isDead)						//Temp work around to avoid entering alert from death
			{						
				EmitSignal(SignalName.Sensed);
				Threat = body;					//FIXME -- currently this will just add the last body as the focused body if that makes sense idk. We should probably draw from list based off of distance of sound/sight in the future?
			}
		}
		
		//}

		/*
		if (!SensedBodies.Contains(body)) {
			SensedBodies.Add(body);
			GD.Print($"{Owner.Name} -- Added {body} to list of sensed bodies");
		}
		*/

	}

	public void SensedRemove(Node3D body) 
	{
		/*
		if (SensedBodies.Contains(body)) {
			SensedBodies.Remove(body);
			GD.Print($"{Owner.Name} -- removed {body} from list of sensed bodies");
		}
		*/

		if ( Threat != null && !HearingArea.OverlapsBody(Threat) && !VisionCone.OverlapsBody(Threat)) //This needs to be totally changed. 
		{
			Threat = null;
		}
	}
}

