using Godot;

public partial class InputControl : Node
{
	[Signal] public delegate void TransitionControlEventHandler(string TransitionTo);

	public const string SKINNING = "Skinning";
	public const string RELOADING = "Reloading";
	public const string DIALOGUE = "Dialogue";
	public const string PAUSE = "PauseMenu";
	public const string INVENTORY = "InventoryUI";

	public override void _Ready() {
		Events.Instance.BeginSkinning += (skinning_scene) => EnterSkinning();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//INVENTORY
		if (Input.IsActionJustPressed("Inventory")) {
			EmitSignal(SignalName.TransitionControl, INVENTORY);
		}

		if (Input.IsActionJustPressed("PauseMenu")) {
			EmitSignal(SignalName.TransitionControl, PAUSE);
		}
	}

	public void EnterSkinning() {
		EmitSignal(SignalName.TransitionControl, SKINNING);
	}
}

