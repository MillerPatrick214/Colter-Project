using Godot;
using System;


//FIXME -- BASCIALLY EVERYTHING IN HERE RIGHT NOW IS FOR DEBUG.
//I think it's pretty dumb for us to be processing input events in this node

public partial class InteractEvent : Control
{

	[Signal]
	public delegate void PauseMouseInputEventHandler(bool isActive);
	bool isActive;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("InteractWorld")) {
			isActive = !isActive;
		}	
		if (isActive) {Active();} else {inactive();}
	}

	public void inactive() {
		EmitSignal(SignalName.PauseMouseInput, false);
		Input.MouseMode = Input.MouseModeEnum.Captured; 
		Hide();
	}

	public void Active() {
		EmitSignal(SignalName.PauseMouseInput, true);
		Input.MouseMode = Input.MouseModeEnum.Confined;
		Show();

	}
}
