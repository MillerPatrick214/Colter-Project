using Godot;
using System;

public partial class lead_ball : RigidBody3D
{	
	[Export]
	float Damage = 4.5f;
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

	public void DamageNPC(Node Body) {

		GD.Print(Body, Body.GetType());

		if (Body is NPCBase npcHit) { //this casts to var npc hit directly
			GD.Print($"Hit {npcHit}");
			npcHit.DamageHealth(Damage);
		}
	}
}
	
