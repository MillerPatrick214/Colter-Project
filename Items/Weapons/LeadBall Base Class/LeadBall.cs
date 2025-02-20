using Godot;
using System;

public partial class LeadBall : RigidBody3D
{	
	[Export]
	public virtual float Damage {get; set;} = 0.0f;
	
	[Export]
	public Area3D HurtBox;
	float CurrentVelocity = 0.0f;
	Vector3 collision_pos;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		HurtBox = GetNode<Area3D>("HurtBox");
		HurtBox.AreaEntered += Attack;
		CollisionMask = 2;
		

		SceneTreeTimer timer = GetTree().CreateTimer(3.0f);		//one shot timer. No node. 
		timer.Timeout += Delete;  
		//BodyEntered += DamageNPC;
		
	}

	public void Delete() {		// Deletes self after Timer runs out. 
		QueueFree();
	}

	/*
	public override void  _IntegrateForces(PhysicsDirectBodyState3D state)
	{
		if (state.GetContactCount() > 0)
		{
			collision_pos = state.GetContactLocalPosition(0);
		}
	}
	
	public void DamageNPC(Node Body) {				//This mapy need to be changed to reflect location damage but that wil largely happen NPC-side.

		if (Body is NPCBase npcHit) { //this casts to var npc hit directly				This will need to apply to player as well
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
	*/

	public void Attack(Area3D area)
	{
		if (area is HitBoxComponent hit_box)
		{
			hit_box.Damage(Damage);
			GD.Print($"{Name} Struck {hit_box.GetParent().Name} for {Damage} damage!");
			Delete();
		}
	}
}