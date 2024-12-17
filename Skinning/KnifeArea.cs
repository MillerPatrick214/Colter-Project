using Godot;
using System;

public partial class KnifeArea : Area2D
{
	[Signal]
	public delegate void MouseOnKnifeEventHandler(bool isTrue);
	bool isMouseOnKnife;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MouseEntered += () => {
			GD.Print("Mouse entered KnifeArea");
			EmitSignal(SignalName.MouseOnKnife, true);
		};
		MouseExited += () => {
			GD.Print("Mouse exited KnifeArea");
		};

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}


}
