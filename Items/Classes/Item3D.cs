using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Item3D : RigidBody3D //base for all items and tools visible in the game world
{

	[Export]
	public bool IsInteractable = true;

	[Export]
	
	public Godot.Collections.Array<NodePath> MeshNodes;	//Set in editor, this holds all meshs that represent the item3D. Facilitates flipping vis layers immensly and eliminates need for recursion to find these nodes

	[Export]
	public InventoryItem ItemResource {get; set;}

	[Export]
	public virtual bool isHeld {get; set;} = false;

	public Item3D() {}
	public Item3D(bool isHeld)
	{
		this.isHeld = isHeld;
	}

	public override void _Ready()
	{
		if(IsInsideTree())
		{
			GD.PrintErr("ITEM3D._READY STARTED");
			SetFreezeEnabled(true);
			SetHeld(isHeld);
			SetVis(isHeld);
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
			SetVis(held);
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

/*
	public Godot.Collections.Array<Node> GetChildrenRecursive(Node node)
	{
		Godot.Collections.Array<Node> results = new Godot.Collections.Array<Node>();

		foreach (Node child in node.GetChildren()){
			if (child != null)
			{
				results.Add(child);
				results.AddRange(GetChildrenRecursive(child));
			}
		}
		return results;
	}
*/
	public void SetVis(bool is_held)
	{
		foreach (NodePath child_path in MeshNodes)
		{
			Node child = GetNodeOrNull<Node>(child_path);

			if (GetNode(child_path) is MeshInstance3D vis)
			{
				vis.SetLayerMaskValue(2, is_held);
				vis.SetLayerMaskValue(1, !is_held);
			}
			else if (child == null)
			{
				GD.PrintErr("WE'RE FUCKED COULDN'T GET CHILD IT'S NULL ITEM3D bitCH");
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
