using Godot;
using System;

public partial class Events : Node
{
	public static Events Instance;

	[Signal]
	public delegate void PlayerRayCastEventHandler(GodotObject InteractableObject);

	[Signal]
	public delegate void ChangeIsInteractingEventHandler(bool isInteracting);

	[Signal]
	public delegate void ChangeIsAimingEventHandler(bool isAiming);

	[Signal]
	public delegate void PickUpEventHandler(InventoryItem ItemUsable);

	[Signal]
	public delegate void BeginSkinningEventHandler(PackedScene SkinningScene);

    [Signal]
    public delegate void InventoryChangedEventHandler();


	public override void _Ready() 
	{
		Instance = this;
	}

}
