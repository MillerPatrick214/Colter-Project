using Godot;
using System;
public partial class HerdComponent : Area3D
{

	[Export] public float separation_factor = .5f; //by what factor do we steer away
	[Export] public float basis_matching_factor = .2f; //by what factor do we match velocity
	[Export] public float centering_factor = .05f; //by what factor do we go towards center
	[Export] public float personal_space = 3.0f; //distance at which we steer away from other capy
	[Export] public float herd_radius = 4.0f;

	[Export] float frequency_bias;
	[Export]float bias_magnitude = 5;
	float start_bias;

	Timer timer;
	Area3D Hearing;
	[Export]
	public Animal Parent;
	StringName SpeciesGroup;					//make a group for each species that will filter what species is being added to the list;

	//public Herd Herd;

	public Godot.Collections.Array<HerdComponent> Boids; 

    public override void _EnterTree()
    {
		timer = new();
		AddChild(timer);
		timer.Start(1);

		timer.Timeout += GenerateBias;

		//RandomNumberGenerator rand = new();
		//start_bias = rand.RandfRange(-2f * Mathf.Pi, -2f * Mathf.Pi);
		//frequency_bias = rand.RandfRange(-.05f, .05f);
		


		Boids = new();
		Parent = (Animal)GetParent<Node3D>();
		if (Parent == null) GD.PrintErr($"Error HerdComponent({GetPath()} No parent assigned!)");
		this.AddToGroup(Parent.SpeciesGroup);
		Hearing = Parent.HearingArea;
		
		Hearing.AreaEntered += (area) => AddBoid(area);
		Hearing.AreaExited += (area) => RemoveBoid(area);

		Godot.Collections.Array<Area3D> init_areas = Hearing.GetOverlappingAreas();
		foreach (Area3D area in init_areas) 
		{
			if (!(area is HerdComponent)) continue;
			//if (area as HerdComponent == this) continue;		idk if we should have this, I doubt it can even pick itself up.
			AddBoid(area);
		}
	}

	public void AddBoid(Area3D area)
	{
		if (!(area is HerdComponent cast_comp)) return;
		if (!Boids.Contains(cast_comp)) Boids.Add(cast_comp);
	}

	public void RemoveBoid(Area3D area)
	{
		if (!(area is HerdComponent cast_comp)) return;
		if (Boids.Contains(cast_comp)) Boids.Remove(cast_comp);
	}

	public Vector3 Separation()
	{
		if (Boids == null) return Vector3.Zero;

		float close_dx = 0;
		float close_dz = 0;
		
		
		foreach (HerdComponent other_comp in Boids)
		{
			float distance = (this.GlobalPosition - other_comp.GlobalPosition).Length();

			if (distance == 0) continue;

			if (distance < personal_space)
			{
				float inv_dist_square = 1/Mathf.Pow(distance, 2);
				close_dx += (this.GlobalPosition.X - other_comp.GlobalPosition.X) * inv_dist_square;
				close_dz += (this.GlobalPosition.Z - other_comp.GlobalPosition.Z) * inv_dist_square;
			}
		}

		Vector3 vect = new Vector3(close_dx * separation_factor, 0, close_dz * separation_factor);
		return vect;
	}

	public void GenerateBias()
	{
		RandomNumberGenerator rand = new();
		start_bias = rand.RandfRange(-2f * Mathf.Pi, -2f * Mathf.Pi);
		frequency_bias = rand.RandfRange(-.05f, .05f);
		bias_magnitude =  rand.RandfRange(-20, 20);
		timer.Start(5);
	}

	public Vector3 Alignment()
	{
		if (Boids.Count == 0)  return Vector3.Zero;

		float x_avg_vel = 0;
		float z_avg_vel = 0;
		int count = 0;																		//TEST. Otherwise use Boids.Count
		
		foreach (HerdComponent other_comp in Boids)
		{
			float distance = (this.GlobalPosition - other_comp.GlobalPosition).Length();	//TEST
			if (distance > personal_space && distance < herd_radius)						//TEST	
			{
				x_avg_vel += other_comp.Parent.Velocity.X;
				z_avg_vel += other_comp.Parent.Velocity.Z;
				count += 1;																	//TEST
			}

		}
		if (count != 0)
		{
			x_avg_vel = x_avg_vel/count;
			z_avg_vel = z_avg_vel/count;
		}

		Vector3 vect = new(x_avg_vel*basis_matching_factor, 0, z_avg_vel*basis_matching_factor);

		return vect;
	}

	public Vector3 Cohesion()
	{
		if (Boids.Count == 0)  return Vector3.Zero;

		float x_pos_avg = 0;
		float z_pos_avg = 0;
		int count = 0;																		//TEST. Otherwise use Boids.Count
		foreach (HerdComponent other_comp in Boids)
		{
			float distance = (this.GlobalPosition - other_comp.GlobalPosition).Length();	//TEST
			if (distance > personal_space && distance < herd_radius)
			{
				x_pos_avg += other_comp.GlobalPosition.X;
				z_pos_avg += other_comp.GlobalPosition.Z;
				count += 1;																	//TEST
			}
		}	
		if (count != 0)																		//TEST
		{
			x_pos_avg = x_pos_avg/count;												

			z_pos_avg = z_pos_avg/count;												

		}

		Vector3 center_vect = new(x_pos_avg, 0, z_pos_avg);

		Vector3 to_center_vect = center_vect - this.GlobalPosition;
		return to_center_vect * centering_factor;
	}

	public Vector3 GetBoidVelocity(double delta) 
	{
		float curr_bias = bias_magnitude * MathF.Sin(start_bias + frequency_bias * (float)delta);
		Vector3 vel_vect;

		vel_vect = Separation();
		vel_vect += Alignment();
		vel_vect += Cohesion();

		if (vel_vect != Vector3.Zero || curr_bias != 0)
		{
			//vel_vect = vel_vect.Rotated(Vector3.Up, Mathf.DegToRad(curr_bias));
		}

		return vel_vect;
	}
}


