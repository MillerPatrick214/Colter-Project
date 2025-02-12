using Godot;
using System;

public partial class Level : Node
{
	public Vector3 player_start;
	public override void _Ready()
	{
		Marker3D mark = GetNodeOrNull<Marker3D>("PlayerStart");
		if (mark == null) {GD.PrintErr("Error Level: PlayerStartMarker returned null");}
		else {player_start = mark.GlobalPosition;}
	}


}
