using System;
using Godot;

public partial class Animal : NPCBase
{
	[Export] public Dietary DietaryHabits;
	[Export] public bool isHerding = false;
	[Export] public StringName SpeciesGroup;					//make a group for each species that will filter what species is being added to the list;
	[Export] public string SkinningScenePath {get; set;} = "res://Skinning/DeerSkinTest.tscn";

	
	public HerdComponent HerdComponent;
	string herd_comp_path = "uid://ccygd4wouya0s";
	
	public enum Dietary
	{
		CARNI_PREDATOR,
		CARNI_SCAVANGER,
		OMNI_PREDATOR,
		OMNI_SCAVANGER,
		VEGETARIAN
	}

	PackedScene SkinningScene;
	public override void _Ready()
	{
		if (isHerding)
		{
			HerdComponent = GetNodeOrNull<HerdComponent>("HerdComponent");
			if (HerdComponent == null)
			{
			HerdComponent = GD.Load<PackedScene>(herd_comp_path).Instantiate() as HerdComponent;
			AddChild(HerdComponent);
			}				
		}
		
		base._Ready();
		
		SkinningScene = GD.Load<PackedScene>(SkinningScenePath); // load skinning scene;

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
		if (InteractComponent != null)
		{
			InteractComponent.CurrentInteractMode = InteractComponent.InteractMode.SKIN;
		}
	}
}

