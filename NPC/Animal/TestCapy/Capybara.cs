using Godot;

[Tool]
public partial class Capybara : NPCBase
{
	PackedScene SkinningScene;
	string currState = ""; 

	public override void _Ready()
	{
		if (Engine.IsEditorHint())
		{
			return;
		}

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

	public PackedScene GetSkinScene()
	{
		return SkinningScene;
	}

    public override void Interact() {
		if (SkinningScene == null) {
			GD.Print("Capybara: Error, skinning scene came back as null");
		}
		Events.Instance.EmitSignal(Events.SignalName.BeginSkinning, SkinningScene);
	}


}

