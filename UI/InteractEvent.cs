using Godot;
using System;


//FIXME -- BASCIALLY EVERYTHING IN HERE RIGHT NOW IS FOR DEBUG.
//I think it's pretty dumb for us to be processing input events in this node


public partial class InteractEvent : Control
{

	[Signal]
	public delegate void PauseMouseInputEventHandler(bool isActive);
	bool isActive = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();

	}
	public void Inactive() {
		EmitSignal(SignalName.PauseMouseInput, false);
		Events.Instance.EmitSignal(Events.SignalName.ChangeIsInteracting, false);
		Input.MouseMode = Input.MouseModeEnum.Captured; 
		Hide();
	}

	public void Active() {
		EmitSignal(SignalName.PauseMouseInput, true);
		Events.Instance.EmitSignal(Events.SignalName.ChangeIsInteracting, true);
		Input.MouseMode = Input.MouseModeEnum.Confined;
		Show();
	}
}