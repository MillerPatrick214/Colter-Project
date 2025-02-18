using Godot;


public partial class UI : Control
{
	InputControl InputControl;

    public override void _Ready()
    {
		InputControl = GetNodeOrNull<InputControl>("InputControl");

		InputControl.TransitionControl += (newUI) => TransitionUI(newUI);
    }
    Control CurrentControl;

	public void TransitionUI(string NewControlPath)
	{
		if (CurrentControl != null)
		{
			CurrentControl.Hide();
		}

		CurrentControl = GetNode<Control>(NewControlPath);
		CurrentControl.Show();
	}
}

