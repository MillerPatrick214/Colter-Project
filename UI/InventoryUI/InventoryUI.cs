using Godot;

public partial class InventoryUI : Control
{
	public override void _Ready()
	{
		Visible = false;
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Inventory")) {
			bool is_hidden = !Visible;	
			if (is_hidden) 
			{
				Events.Instance.EmitSignal(Events.SignalName.ChangeIsInteracting, true);
				Show();
			}

			else {
				Events.Instance.EmitSignal(Events.SignalName.ChangeIsInteracting, false);
				Hide();
			}
		}
	}
}
