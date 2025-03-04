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
	[Export] OptionButton MaxFPSButton;
	[Export] Line2D StrikeLine;
	
	// [ExportGroup("Audio")]

	private Godot.Collections.Array<string> windowModes = new();
	private Godot.Collections.Array<string> resolutions = new();
	private Godot.Collections.Array<int> fpss = new();

	public override void _Ready()  {
		StrikeLine.Hide();
		windowModes = Settings.Instance.WindowModes;
		resolutions = Settings.Instance.Resolutions;
		fpss = Settings.Instance.FPSs;

		LookSensitivitySlider.ValueChanged += OnLookSensitivitySlider;
		LookSensitivitySpinBox.ValueChanged += OnLookSensitivitySpinBox;

		WindowModeButton.ItemSelected += OnWindowModeButton;
		ResolutionButton.ItemSelected += OnResolutionButton;
		VSyncButton.Pressed += OnVSyncButton;
		MaxFPSButton.ItemSelected += OnMaxFPSButton;

		LoadSettings();
	}

	// Populates settings values from user settings file
	public void LoadSettings() {
		// Look Sensitivity HSlider and SpinBox
		LookSensitivitySlider.Value = (double)Settings.Instance.GetSetting("gameplay", "look_sensitivity");
		LookSensitivitySpinBox.Value = LookSensitivitySlider.Value;

		// Window Mode OptionButton
		string currentMode = (string)Settings.Instance.GetSetting("graphics", "window_mode");
		WindowModeButton.Clear();
		for (int i = 0; i < windowModes.Count; i++) {
			WindowModeButton.AddItem(windowModes[i], i);
			if (windowModes[i] == currentMode) { WindowModeButton.Select(i); }
		}
		
		// Resolution OptionButton
		string currentRes = (string)Settings.Instance.GetSetting("graphics", "resolution");
		ResolutionButton.Clear();
		for (int i = 0; i < resolutions.Count; i++) {
			ResolutionButton.AddItem(resolutions[i], i);
			if (resolutions[i] == currentRes) { ResolutionButton.Select(i); }
		}

		// VSync CheckButton
		if ((bool)Settings.Instance.GetSetting("graphics", "vsync") == true) {
			VSyncButton.ButtonPressed = true;
			MaxFPSButton.Hide();
			StrikeLine.Show();
		}
		else { 
			VSyncButton.ButtonPressed = false;
			MaxFPSButton.Show();
			StrikeLine.Hide();
		}

		// MaxFPS OptionButton
		int currentFPS = (int)Settings.Instance.GetSetting("graphics", "max_fps");
		MaxFPSButton.Clear();
		MaxFPSButton.AddItem("Unlimited", 0);
		for (int i = 1; i < fpss.Count - 1; i++) {
			MaxFPSButton.AddItem(fpss[i].ToString(), i);
			if (fpss[i] == currentFPS) { MaxFPSButton.Select(i); }
		}

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
			MaxFPSButton.Hide();
			StrikeLine.Show();
		}
		else {
			MaxFPSButton.Show();
			StrikeLine.Hide();
		}
	}
	public void OnMaxFPSButton(long item) {  Settings.Instance.SetSetting("graphics", "max_fps" , (int)item); }

	// [audio]
}