using DialogueManagerRuntime;
using Godot;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

public partial class InteractComponent : Area3D
{

	Node ParentNode;

	[Export]
	public InteractMode CurrentInteractMode;


	[Export]
	public Resource SkinBehavior;

	[Export]
	public InteractTalkLogic InteractTalkLogic;

	[Export]
	public Resource LootBehavior;

	[Export]
	public InteractPickUpLogic InteractPickUpLogic;

	public enum InteractMode
	{
		NONE,
		SKIN,
		TALK,
		LOOT,
		PICKUP
	}
	
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
    	while (!IsInsideTree())
		{
			await Task.Delay(10); // Small delay to avoid blocking the thread
		}
		ParentNode = GetParent();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Interact()
	{
		switch (CurrentInteractMode)
		{
			case (InteractMode.NONE):
			return;
			
			case (InteractMode.SKIN):
			return;

			case(InteractMode.TALK):
			InteractTalkLogic.Interact();
			return;

			case(InteractMode.LOOT):
			return;

			case(InteractMode.PICKUP):
			if (ParentNode is Item3D item)
			{
				InteractPickUpLogic.Interact(item);
			}
			return;
		}
	}
}
