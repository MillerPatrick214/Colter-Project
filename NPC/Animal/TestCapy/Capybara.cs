using Godot;

[Tool]
public partial class Capybara : NPCBase
{
	
	// Called when the node enters the scene tree for the first time.
	PackedScene SkinningScene;
	string currState = ""; 
	
	//
	//I think we will likely want a curr List of sensed bodies and a body for the "Threat" that is set by the alert mode once "proof" of that threat is visually confirmed or susometer reaches max?
	//

	//List<Node3D> SensedBodies;


	//so current flow is
	//	1. Added to sensed
	//	2. Rotate VisualArea to include body
	//	3. Adjust RayCast to attempt to find body
	//	4.		If : RayCast finds body, set to flee (and then somehow pass data over to it telling it the direction to flee in? Maybe use nav )
	//	5. 		Else : RayCast Can't find body after x seconds, go back to Idle


	public override void _Ready()
	{

		base._Ready();

		SkinningScene = GD.Load<PackedScene>("res://Skinning/DeerSkinTest.tscn"); // load scene

		if (NavAgent == null) {GD.Print("God damn this is fucked! Capybara: NavAgent is null");}
		else {GD.Print("We're Chuned! NavAgent found successfully");} 

		if (AniTree == null) {GD.Print("Capybara: Fuck AniTree is Null");}

		if (HearingArea == null || VisionCone == null || VisionRay == null) {
			 GD.Print((HearingArea == null) ? "Capybara: HearingArea came back as null" : "");
			 GD.Print((VisionCone == null) ? "Capybara: VisionCone came back as null" : "");
			 GD.Print((VisionRay == null) ? "Capybara: VisionRay came back as null" : "");
		}
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


}

