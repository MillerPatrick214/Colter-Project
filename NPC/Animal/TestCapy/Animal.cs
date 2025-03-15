using System;
using Godot;
using Godot.NativeInterop;
using Microsoft.VisualBasic;

public partial class Animal : NPCBase
{
	[Export] public bool isHerding = false;
	[Export] public StringName SpeciesGroup;					//make a group for each species that will filter what species is being added to the list;
	[Export] public string SkinningScenePath {get; set;} = "res://Skinning/DeerSkinTest.tscn";
	[Export] public AnimalHurtBox AnimalHurtBox {get; set;} 

	//[ExportToolButton("Generate Missing Nodes")] public Callable GenerateBlankNodesButtom => Callable.From(GenerateBlankNodes);
	public NPCBase PreyFocus;
	public HerdComponent HerdComponent;
	string herd_comp_path = "uid://ccygd4wouya0s";
	PackedScene SkinningScene;

	public override void _Ready()
	{
		base._Ready();

		if (isHerding)
		{
			HerdComponent = GetNodeOrNull<HerdComponent>("HerdComponent");
			
			if (HerdComponent == null)
			{
				HerdComponent = GD.Load<PackedScene>(herd_comp_path).Instantiate() as HerdComponent;
				AddChild(HerdComponent);
			}				
		}
				
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

	public void GenerateBlankNodes()
	{

	}

	public override void SensedAdd(Node3D body) 
	{	
		base.SensedAdd(body);
		if (body.GetInstanceId() == this.GetInstanceId()) return;
		if (body is not NPCBase npc ) return;
		if (SpeciesDiet == Dietary.VEGETARIAN) return;

		//GD.PrintErr($" check bool:{body is NPCBase} needs to be true"); 
		//GD.PrintErr($"SpeciesDiet:{SpeciesDiet}");
		//GD.PrintErr($"SpeciesThreatLevel:{SpeciesThreatLevel}");
		//GD.PrintErr($"NPC ThreatLevel:{npc.SpeciesThreatLevel}");

		if (SpeciesThreatLevel > npc.SpeciesThreatLevel)
		{
			if (PreyFocus == null)
			{
				PreyFocus = npc;
				BTPlayer.Blackboard.SetVar("PreyFocus", PreyFocus);
				GD.PrintErr($"PreyFocus:{PreyFocus}");
			
			Godot.Variant VarCheck = BTPlayer.Blackboard.GetVar("PreyFocus", new Godot.Variant());

			//GD.PrintErr($"Debgug blackboard focus {BTPlayer.Blackboard.GetVar("PreyFocus")}");

			if (VarCheck.VariantType is Godot.Variant.Type.Nil)
			{
				//GD.PrintErr($"Error in Animal.cs {GetPath()} Attempted to set BlackBoard object as prey node but returned {VarCheck} in final check!");
				return;
			}
			}
		}
	}

	public override void SensedRemove(Node3D body) 
	{	
		
		base.SensedAdd(body);
		if (body.GetInstanceId() == this.GetInstanceId()) return;
		if (body is not NPCBase npc ) return;
		if (SpeciesDiet == Dietary.VEGETARIAN) return;

		/*
		if (PreyFocus == body) PreyFocus = null;
		
		NPCBase potential_prey = null;
		float min_dist = -1;

		foreach (Node3D other_body in HearingArea.GetOverlappingBodies())
		{	
			if (other_body is NPCBase npc_evaluated  && SpeciesThreatLevel > npc_evaluated.SpeciesThreatLevel)
			{
				if ((npc_evaluated.GlobalPosition - this.GlobalPosition).Length() < min_dist);
				potential_prey = npc_evaluated;
			}
		}
		if (potential_prey != null)
		{
			PreyFocus = potential_prey;
			BTPlayer.Blackboard.SetVar("PreyFocus", PreyFocus);
			return;
		}
		Variant null_var = new Godot.Variant();
		
		BTPlayer.Blackboard.SetVar("PreyFocus", null_var);
		
		// Used for Check
		Variant default_var = new Godot.Variant();
		default_var =  default_var.As<bool>();
		
		Godot.Variant VarCheck = BTPlayer.Blackboard.GetVar("PreyFocus", default_var);

		if (VarCheck.VariantType is not  Godot.Variant.Type.Nil)
		{
			GD.PrintErr($"Error in Animal.cs {GetPath()} Attempted to set BlackBoard object as Nil but returned some object {VarCheck} in final check!");
		}
		*/
		
	}


	public override void Death() {
		base.Death();
		if (InteractComponent != null)
		{
			InteractComponent.CurrentInteractMode = InteractComponent.InteractMode.SKIN;
		}
	}
	

	public void Attack(HitBoxComponent area)
	{
		if (AnimalHurtBox == null) GD.PrintErr($"{GetPath()} Error in Animal::Attack(). Attempted to attack but AnimalHurtBox == null.");
		AnimalHurtBox.Attack(area);
	}
}

