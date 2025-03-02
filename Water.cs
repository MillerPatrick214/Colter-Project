using Godot;

public partial class Water : MeshInstance3D
{
	[Export]
	float default_depth = 50.0f;
	CollisionShape3D water_coll;
	Area3D coll_area;

	bool enter = true;
	bool exit = false; 

	public override void _Ready()
	{
		water_coll = GetNodeOrNull<CollisionShape3D>("Area3D/WaterCollision");		//get water collision
		coll_area = GetNodeOrNull<Area3D>("Area3D");								

		PlaneMesh mesh = this.Mesh as PlaneMesh;
		Vector2 size = mesh.Size;

		BoxShape3D coll_shape = new BoxShape3D();									// create a new box3d
		coll_shape.Size = new Vector3(size.X, default_depth, size.Y);				// set box shape to this size & default depth for water
		water_coll.Shape = coll_shape;												// pass over box to water collision node for collision shape

		water_coll.Position = new Vector3(0, -(default_depth/2), 0);				// set water the node's local position down 1/2 the default depth, as normally it is 1/2 ontop and 1/2 on the bottom split thru center
		coll_area.AreaEntered += (area) => UnderwaterCheck(area, enter);
		coll_area.AreaExited += (area) => UnderwaterCheck(area, exit);
	}
	public void UnderwaterCheck(Area3D area, bool isEnter)
	{
		if (area.IsInGroup("PlayerCamera"))
		{
			Events.Instance.EmitSignal(Events.SignalName.UnderwaterToggle, isEnter);
		}
	}
}
