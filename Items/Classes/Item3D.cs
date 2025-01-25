using Godot;
using System;

public partial class Item3D : RigidBody3D //base for all items and tools visible in the game world
{

	[Export]
	public bool IsInteractable = true;
	
	[Export]
	public Resource ItemResource {get; set;}

	[Export]
	public virtual bool isHeld {get; set;} = false;

	public override void _Ready()
	{
		if(IsInsideTree())
		{
			SetFreezeEnabled(true);

			// Now it's safe to check the Owner
			if (Owner != null && Owner.IsClass("CharacterBody3D")) 
			{
				isHeld = true;
			}
			else
			{
				isHeld = false;
			}

			SetHeld(isHeld);
		}
		else
		{
			GD.Print("Node is not yet inside the scene tree.");
		}
	}

	public void SetHeld(bool held) {
		
		if (held) 
		{
			FreezeMode = FreezeModeEnum.Static;
			SetCollision(false);
		}
		else
		{
			FreezeMode = FreezeModeEnum.Kinematic;
			SetCollision(true);
		}

		Freeze = held;
		
		foreach (Node node in GetChildren()) 
		{
			//GD.PrintErr($"Node getClassRestults: {node.GetClass()}");

			if (node.GetClass() == "AnimationPlayer") 
			{
				AnimationPlayer animationPlayer = GetNodeOrNull<AnimationPlayer>(node.GetPath());
				animationPlayer.Active = held;
			}
			
			if (node.GetClass() == "AnimationTree") 
			{
				AnimationTree animationTree = GetNodeOrNull<AnimationTree>(node.GetPath());
				animationTree.Active = held; 
			}
		}
	}

	public void SetCollision(bool collBool)		//True to enable collision, false to disable. 
	{
		SetCollisionLayerValue(1, collBool);
		SetCollisionLayerValue(2, collBool);
		SetCollisionMaskValue(1, collBool);
		SetCollisionMaskValue(2, collBool);
	}
	
	public void Interact() 
	{
		Events.Instance.EmitSignal(Events.SignalName.PickUp, ItemResource);
		QueueFree();
	}
	
}
