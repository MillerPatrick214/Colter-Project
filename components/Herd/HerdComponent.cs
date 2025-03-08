using Godot;
using System;
public partial class HerdComponent : Area3D
{

	[Export] public float AvoidanceFactor = .3f; //by what factor do we steer away
	[Export] public float turn_degrees = .1f;
	[Export] public float basis_matching_factor = .5f;
	[Export] public float centering_factor = 1f;
	[Export] public float turn_factor = 5f;	//Slows speed while turning;

	float curr_bias = 0;


	Timer timer;
	Area3D Hearing;
	[Export]
	public Animal Parent;
	StringName SpeciesGroup;					//make a group for each species that will filter what species is being added to the list;

	//public Herd Herd;

	public Godot.Collections.Array<HerdComponent> Boids; 

    public override void _EnterTree()
    {
		RandomNumberGenerator rand = new();
		Boids = new();
		Parent = (Animal)GetParent<Node3D>();
		if (Parent == null) GD.PrintErr($"Error HerdComponent({GetPath()} No parent assigned!)");
		this.AddToGroup(Parent.SpeciesGroup);
		Hearing = Parent.HearingArea;

		curr_bias = rand.RandfRange(-.3f, .3f);
		
		Hearing.AreaEntered += (area) => AddBoid(area);
		Hearing.AreaExited += (area) => RemoveBoid(area);

		Godot.Collections.Array<Area3D> init_areas = Hearing.GetOverlappingAreas();
		foreach (Area3D area in init_areas) 
		{
			if (!(area is HerdComponent)) continue;
			if (area as HerdComponent == this) continue;
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
			close_dx += this.GlobalPosition.X - other_comp.GlobalPosition.X;
			close_dz += this.GlobalPosition.Z - other_comp.GlobalPosition.Z;
		}
		Vector3 vect = new Vector3(close_dx*AvoidanceFactor, 0, close_dz*AvoidanceFactor);
		return vect;
	}

	public Vector3 Alignment()
	{
		if (Boids.Count == 0)  return Vector3.Zero;

		float x_avg_vel = 0;
		float z_avg_vel = 0;
		float count = Boids.Count;
		
		foreach (HerdComponent other_comp in Boids)
		{
			if (other_comp == null) {GD.PrintErr("OtherComp null");}
			if (other_comp.Parent == null) {GD.PrintErr("OtherComp parent null");}
			x_avg_vel += other_comp.Parent.Velocity.X;
			z_avg_vel += other_comp.Parent.Velocity.Z;

		}
		x_avg_vel = x_avg_vel/count;
		z_avg_vel = z_avg_vel/count;

		
		Vector3 vect = new(x_avg_vel*basis_matching_factor, 0, z_avg_vel*basis_matching_factor);
		return vect;
	}

	public Vector3 Cohesion()
	{
		if (Boids.Count == 0)  return Vector3.Zero;

		float x_pos_avg = 0;
		float z_pos_avg = 0;
		
		foreach (HerdComponent other_comp in Boids)
		{
			x_pos_avg += other_comp.GlobalPosition.X;
			z_pos_avg += other_comp.GlobalPosition.Z;
		}

		x_pos_avg = x_pos_avg/Boids.Count;
		z_pos_avg = z_pos_avg/Boids.Count;

		Vector3 center_vect = new(x_pos_avg, 0, z_pos_avg);

		Vector3 to_center_vect = center_vect - this.GlobalPosition;
		return to_center_vect * centering_factor;
	}

	public Vector3 GetBoidVelocity(float min_speed, float max_speed, double delta) 
	{
		Vector3 vel_vect;

		vel_vect = Separation();
		vel_vect += Alignment();
		vel_vect += Cohesion();

		vel_vect = vel_vect.Rotated(Vector3.Up, Mathf.DegToRad(curr_bias));
		return vel_vect;
	}
}



