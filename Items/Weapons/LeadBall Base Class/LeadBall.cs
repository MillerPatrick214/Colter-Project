using Godot;
using System;

public partial class LeadBall : RigidBody3D
{	
	[Export]
	public virtual float Damage {get; set;} = 0.0f;
	float CurrentVelocity = 0.0f;
	Vector3 collision_pos;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CollisionMask = 2;

		SceneTreeTimer timer = GetTree().CreateTimer(3.0f);		//one shot timer. No node. 
		timer.Timeout += Delete;  
		BodyEntered += DamageNPC;
	}

	public void Delete() {		// Deletes self after Timer runs out. 
		QueueFree();
	}

	public override void  _IntegrateForces(PhysicsDirectBodyState3D state)
	{
		if (state.GetContactCount() > 0)
		{
			collision_pos = state.GetContactLocalPosition(0);
		}
	}
	public void DamageNPC(Node Body) {				//This mapy need to be changed to reflect location damage but that wil largely happen NPC-side.

		if (Body is NPCBase npcHit) { //this casts to var npc hit directly
			PackedScene decal_scene = GD.Load<PackedScene>("uid://ewsuiyyn4qb0");
			Decal decal_instance = decal_scene.Instantiate<Decal>();

			npcHit.AddChild(decal_instance);

			decal_instance.Basis = Basis.LookingAt(-LinearVelocity); 
			decal_instance.GlobalPosition = collision_pos;
			GD.PrintErr($"Hit {npcHit}");
			npcHit.DamageHealth(Damage);
			GD.PrintErr($"Damage {Damage}");
			Delete();
		}
	}
}