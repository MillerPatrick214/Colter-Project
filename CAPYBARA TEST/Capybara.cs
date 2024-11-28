using Godot;
using System;

public partial class Capybara : NPCBase
{
	
	// Called when the node enters the scene tree for the first time.
	PackedScene SkinningScene;
	
	public override void _Ready()
	{
		SkinningScene = GD.Load<PackedScene>("res://Skinning/DeerSkinTEST.tscn"); // load scene

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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


}

