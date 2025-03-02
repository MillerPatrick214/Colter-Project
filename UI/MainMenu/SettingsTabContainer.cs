using System;
using Godot;

public partial class SettingsTabContainer : TabContainer
{
	[ExportGroup("Gameplay")]
	[ExportSubgroup("Look Sensitivity")]
	[Export] HSlider LookSensitivitySlider;
	[Export] SpinBox LookSensitivitySpinBox;

	// [ExportGroup("Controls")]
	
	[ExportGroup("Graphics")]
	[Export] OptionButton WindowModeButton;
	[Export] OptionButton ResolutionButton;
	[Export] CheckBox VSyncButton;
	[Export] SpinBox MaxFPSSpinBox;
	[Export] Label MaxFPSLabel;
	[Export] Line2D StrikeLine;
	
	// [ExportGroup("Audio")]

	public override void _Ready()  {
		StrikeLine.Hide();

		LookSensitivitySlider.ValueChanged += OnLookSensitivitySlider;
		LookSensitivitySpinBox.ValueChanged += OnLookSensitivitySpinBox;

		WindowModeButton.ItemSelected += OnWindowModeButton;
		ResolutionButton.ItemSelected += OnResolutionButton;
		VSyncButton.Pressed += OnVSyncButton;
		MaxFPSSpinBox.ValueChanged += OnMaxFPSSpinBox;

		LoadSettings();
	}

	// Populates settings values from user settings file
	public void LoadSettings() {
		// Look Sensitivity HSlider and SpinBox
		LookSensitivitySlider.Value = (double)Settings.Instance.GetSetting("gameplay", "look_sensitivity");
		LookSensitivitySpinBox.Value = LookSensitivitySlider.Value;

		// Window Mode OptionButton
		string currentMode = (string)Settings.Instance.GetSetting("graphics", "window_mode");
		Enum.TryParse(currentMode, true, out Settings.WindowDisplayMode mode);
		WindowModeButton.Select((int)mode);
		
		// Resolution OptionButton
		string currentRes = (string)Settings.Instance.GetSetting("graphics", "resolution");
		currentRes = currentRes.Replace(" ", "");

		Enum.TryParse("Res_" + currentRes, true, out Settings.WindowDisplayMode res);
    	ResolutionButton.Select((int)res);


		// VSync CheckButton
		if ((string)Settings.Instance.GetSetting("graphics", "vsync") == "true") {
			VSyncButton.ButtonPressed = true;
			MaxFPSSpinBox.Hide();
			StrikeLine.Show();
		}
		else { 
			VSyncButton.ButtonPressed = false;
			MaxFPSSpinBox.Show();
			StrikeLine.Hide();
		}

		// MaxFPS SpinBox
		MaxFPSSpinBox.Value = (double)Settings.Instance.GetSetting("graphics", "max_fps");

	}

	// [gameplay]
	public void OnLookSensitivitySlider(double value) {
		LookSensitivitySpinBox.Value = value;
	}

	public void OnLookSensitivitySpinBox(double value) {
		LookSensitivitySlider.Value = value;
		Settings.Instance.SetSetting("gameplay", "look_sensitivity", value);
	}

	// [controls]

	// [graphics]
	public void OnWindowModeButton(long item) { Settings.Instance.SetSetting("graphics", "window_mode", WindowModeButton.GetItemText((int)item)); }
	public void OnResolutionButton(long item) { Settings.Instance.SetSetting("graphics", "resolution", ResolutionButton.GetItemText((int)item)); }
	public void OnVSyncButton() { 
		Settings.Instance.SetSetting("graphics", "vsync", VSyncButton.ButtonPressed);
		if (VSyncButton.ButtonPressed == true) {
			MaxFPSSpinBox.Hide();
			StrikeLine.Show();
		}
		else {
			MaxFPSSpinBox.Show();
			StrikeLine.Hide();
		}
	}
	public void OnMaxFPSSpinBox(double value) {  Settings.Instance.SetSetting("graphics", "max_fps" , value); }

	// [audio]
}