using Godot;
using System;

public partial class LeadBall : RigidBody3D
{	
	[Export]
	public virtual float Damage {get; set;} = 0.0f;
	float CurrentVelocity = 0.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SceneTreeTimer timer = GetTree().CreateTimer(3.0f);		//one shot timer. No node. 
		timer.Timeout += Delete;  
		BodyEntered += DamageNPC;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void Delete() {		// Deletes self after Timer runs out. 
		QueueFree();
	}

	public void DamageNPC(Node Body) {				//This mapy need to be changed to reflect location damage but that wil largely happen NPC-side

		GD.Print(Body, Body.GetType());

		if (Body is NPCBase npcHit) { //this casts to var npc hit directly
			GD.Print($"Hit {npcHit}");
			npcHit.DamageHealth(Damage);
		}
	}
}
	
