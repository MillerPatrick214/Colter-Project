using Godot;
using System;

public partial class pause_menu : Control
{
	// Called when the node enters the scene tree for the first time.

	bool Opened = false;
	Button Resume;
	Button Settings;
	Button Quit;
	
	public override void _Ready()
	{
		this.ProcessMode = ProcessModeEnum.Always;			//basically this means that this node's script is always processed -- even when paused. 
		Hide();
		Resume = GetNode<Button>("MarginContainer/VBoxContainer/Resume");
		Resume.Pressed += OnResumePressed;

		Settings = GetNode<Button>("MarginContainer/VBoxContainer/Settings");
		Settings.Pressed += OnSettingsPressed;

		Quit = GetNode<Button>("MarginContainer/VBoxContainer/Quit");
		Quit.Pressed += OnQuitPressed;

		


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("PauseMenu")) {
			if (!Opened) {
				PauseMenu();
			}
			else {
				Unpause();
			}
		}
	}

	public void PauseMenu() {
		GetTree().Paused = true;
		Show();
		Opened = true;
		Input.MouseMode = Input.MouseModeEnum.Confined;

	}

	public void Unpause() {
		GetTree().Paused = false;
		Hide();
		Opened = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public void OnResumePressed() {
		Unpause();

	}


	public void OnSettingsPressed() {

	}

	public void OnQuitPressed() {
		GetTree().Quit();
	}

}
