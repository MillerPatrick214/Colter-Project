using Godot;
using System;

public partial class TestDoubleBarrel : Node3D
{
	// Called when the node enters the scene tree for the first time.
	[Signal]
	public delegate void FireEventHandler();
	CharacterBody3D Char;
	AnimatedSprite3D ChildSprite;

	CamPivot CamPivotNode;
	Timer timer;

	bool AimingState; 

	public override void _Ready()
	{

		AimingState = false;
		Char = GetTree().GetNodesInGroup("PlayerCharacter")[0] as CharacterBody3D;
		ChildSprite = GetNodeOrNull<AnimatedSprite3D>("AnimatedSprite3D");
		CamPivotNode = Char.GetNodeOrNull<CamPivot>("CamPivot");
		timer = GetNodeOrNull<Timer>("Timer");

		if (Char == null || ChildSprite == null || CamPivotNode == null) {
			GD.Print("TestDoubleBarrel: Error, some or all nodes returned null.");
		}

		CamPivotNode.AimSignal += Aiming;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("UseItem") && AimingState && timer.IsStopped()) {
			EmitSignal(SignalName.Fire);
			timer.Start(5);
			GD.Print("Emitted Fire Signal");
		}
	}

	public void Aiming(bool isAiming) 
	{
		AimingState = isAiming;
	}
}
