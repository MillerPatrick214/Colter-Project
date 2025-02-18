using Godot;


public partial class UI : Control
{
	Node InputControl;

    public override void _Ready()
    {
		InputControl = GetNodeOrNull<Node>("InputControl");

		InputControl.TransitionUI += (newUI) => 
    }
    Control CurrentControl;

	public const string SKINNING = "Skinning";
	public const string RELOADING = "Reloading";
	public const string DIALOGUE = "Dialogue";
	public const string PAUSE = "PauseMenu";
	public const string INVENTORY = "InventoryUI";

	public void TransitionUI(Control NewControl)
	{
		if (CurrentControl != null)
		{
			CurrentControl.Hide();
		}

		CurrentControl = NewControl;
		CurrentControl.Show();
	}
}

