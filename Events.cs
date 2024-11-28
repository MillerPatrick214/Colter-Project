using Godot;
using System;

public partial class Events : Node
{
	public static Events Instance;

	[Signal]
	public delegate void PlayerRayCastEventHandler(GodotObject InteractableObject);

	[Signal]
	public delegate void ChangeIsInteractingEventHandler(bool isInteracting);

//	[Signal]
//	public delegate void NPCInteractEventHandler(NPCBase NPC);

	[Signal]
	public delegate void PickUpEventHandler(PackedScene ItemUsable);

	[Signal]
	public delegate void BeginSkinningEventHandler(PackedScene SkinningScene);


	public override void _Ready() 
	{
		Instance = this;
	}

}
