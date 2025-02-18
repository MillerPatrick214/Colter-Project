using Godot;
using System;
using System.Reflection.Metadata;

public partial class InputControl : Node
{
	[Signal]
	public delegate void TransitionControlEventHandler(string TransitionTo);

	public const string SKINNING = "Skinning";
	public const string RELOADING = "Reloading";
	public const string DIALOGUE = "Dialogue";
	public const string PAUSE = "PauseMenu";
	public const string INVENTORY = "InventoryUI";

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//INVENTORY
		if (Input.IsActionJustPressed("Inventory")) {
			EmitSignal(SignalName.TransitionControl, INVENTORY);
		}

		if (Input.IsActionJustPressed(""))


		if (Input.IsActionJustPressed("PauseMenu")) {
		}
	}
}
