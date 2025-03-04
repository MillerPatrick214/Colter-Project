using Godot;

[GlobalClass]
public partial class Level : Node3D
{
	[Export]
	public Vector3 player_start;
	
	public override void _Ready()
	{
		Marker3D mark = GetNodeOrNull<Marker3D>("PlayerStart");
		if (mark == null) {GD.PrintErr("No 'PlayerStart' Marker3D node found as a direct child. Using exported editor property as start");}
		else {player_start = mark.GlobalPosition;}
	}
}
