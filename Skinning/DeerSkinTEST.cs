using Godot;
using System;

public partial class DeerSkinTEST : Skinnable
{
	[Signal]
	public delegate void MouseOnSkinEventHandler(bool isTrue);
	

	Area2D SkinArea; 
	public override void _Ready()
	{
		SkinArea = GetNodeOrNull<Area2D>("SkinArea");
		GD.Print((SkinArea == null) ? "" : "DeerSkinTEST: SkinArea node returned null");

		SkinArea.MouseEntered += () => EmitSignal(SignalName.MouseOnSkin, true);
		SkinArea.MouseExited += () => EmitSignal(SignalName.MouseOnSkin, false);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}

