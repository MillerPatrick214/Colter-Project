using Godot;


public partial class UI : Control
{
	
	InputControl InputControl;
	Control CurrentControl;
	
	PauseMenu PauseMenu;

    public override void _Ready()
    {
		InputControl = GetNodeOrNull<InputControl>("InputControl");

		InputControl.TransitionControl += (newUI) => TransitionUI(newUI);
		CurrentControl = null;

		PauseMenu = GetNodeOrNull<PauseMenu>("PauseMenu");
		PauseMenu.PauseResumePressed += PauseResumed;
    }

	public void TransitionUI(string NewControlPath)
	{
		Control new_control = GetNodeOrNull<Control>(NewControlPath);

		if (CurrentControl == new_control)
		{
			CurrentControl.Hide();
			Events.Instance.EmitSignal(Events.SignalName.ChangeIsInteracting, false);
			Input.MouseMode = Input.MouseModeEnum.Captured;
			if (CurrentControl is PauseMenu pause_menu)
			{
				pause_menu.Unpause();
			}
			CurrentControl = null;
		}
		
		else if (CurrentControl == null)
		{
			Events.Instance.EmitSignal(Events.SignalName.ChangeIsInteracting, true);
			CurrentControl = new_control;
			CurrentControl.Show();
			if (CurrentControl is PauseMenu pause_menu)						//This is a lazy fix. Will need more thought on how to implement Pause Menu handling
			{
				pause_menu.Pause();
			}
		}
		
		else 
		{
			Input.MouseMode = Input.MouseModeEnum.Confined;
			Events.Instance.EmitSignal(Events.SignalName.ChangeIsInteracting, true);
			CurrentControl.Hide();
			CurrentControl = GetNode<Control>(NewControlPath);
			CurrentControl.Show();
			if (CurrentControl is PauseMenu pause_menu)
			{
				pause_menu.Pause();
			}
			
		}

	}

	public void PauseResumed()
	{
		CurrentControl.Hide();
		CurrentControl = null;

	}
}

