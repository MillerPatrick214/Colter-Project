using Godot;

public partial class Settings : Node
{
	public static Settings Instance { get; private set; }
	[Signal] public delegate void SettingChangedEventHandler();

	string defaultPath = "res://Settings/DefaultSettings.cfg";
	ConfigFile defaultFile = new ConfigFile();
	Godot.Collections.Dictionary<string, Variant> defaultDict = new();

	string settingsPath = "user://settings/settings.cfg";
	ConfigFile SettingsFile = new ConfigFile();

	Godot.Collections.Dictionary<string, Callable>  settingActionDict = new();

	Godot.Collections.Dictionary<string, string> actionQueue = new();

	public enum WindowDisplayMode {
		Fullscreen = 3,
		Windowed = 1,
		Borderless = 2,
	};

	public enum ProjectResolution {
		Res_3840x2160,	// 0
		Res_3440x1440,	// 1
		Res_2560x1600,	// 2
		Res_2560x1440,	// 3
		Res_1920x1080,	// 4
		Res_1920x1200,	// 5
		Res_1366x768,	// 6
	};
	


	public override void _Ready() {
		Instance = this;
		defaultFile.Load(defaultPath);
		SettingsFile.Load(settingsPath);

		if (SettingsFile == null) {
			if (!FileAccess.FileExists(settingsPath)) {
				GD.Print($"[color=red]Settings: [/color] No file found at {settingsPath}");
				GD.Print($"[color=red]Settings: [/color] Generating default settings.");

				FileAccess defaultFile = FileAccess.Open(defaultPath, FileAccess.ModeFlags.Read);
				string content = defaultFile.GetAsText();
				FileAccess newFile = FileAccess.Open(settingsPath, FileAccess.ModeFlags.Write);
				newFile.StoreString(content);
				SettingsFile.Load(settingsPath);
			}
			else { GD.PrintErr($"Can't load ConfigFile at {settingsPath}"); }
		}

		settingActionDict = new() {
			// [gameplay]
			// [graphics]
			{"window_mode", new Callable(this, MethodName.WindowMode)},
			{"resolution", new Callable(this, MethodName.Resolution)},
			{"vsync", new Callable(this, MethodName.VSync)},
			{"max_fps", new Callable(this, MethodName.MaxFPS)},
			// [audio]
		};

		LoadDefaults();
	}

	private void LoadDefaults() {
		foreach (string section in defaultFile.GetSections()) {
			foreach (string key in defaultFile.GetSectionKeys(section)) {
				defaultDict[key] = defaultFile.GetValue(section, key);
			}
		}

		foreach (string setting_section in SettingsFile.GetSections()) {
			foreach (string setting_key in SettingsFile.GetSectionKeys(setting_section)) {
				if (!defaultFile.HasSectionKey(setting_section, setting_key)) {
					SetSetting(setting_section, setting_key, defaultFile.GetValue(setting_section, setting_key));
				}
			}
		}
		SaveSettings();
	}

	/// <summary>
	/// Gets a setting value from the active ConfigFile settings file object.
	/// </summary>
	/// <param name="section">The settings.ini section string.</param>
	/// <param name="key">The settings.ini key string.</param>
	/// <returns>The setting value.</returns>
	public Variant GetSetting(string section, string key) {
		return SettingsFile.GetValue(section, key);
	}

	/// <summary>
	/// Sets a setting value to the active ConfigFile settings file object. 
	// NOTE: It does not apply those settings to the settings.ini user file or implement them. Run SaveSettings() to save and apply active changes.
	/// </summary>
	/// <param name="section">The settings.ini section string.</param>
	/// <param name="key">The settings.ini key string.</param>
	/// <param name="value">The settings value to be applied.</param>
	public void SetSetting(string section, string key, Variant value) {
		section = section.ToLower();
		key = key.ToLower();

		if (value.Equals(GetSetting(section, key))) { 
			if (actionQueue.ContainsKey(key)) { actionQueue.Remove(key); }
			return;
		 }

		SettingsFile.SetValue(section, key, value);
		if (!actionQueue.ContainsKey(key)) {
			actionQueue.Add(key, section);
		}
		EmitSignal(SignalName.SettingChanged);
	}

	/// <summary>
	/// Saves the ConfigFile object to settings.ini and runs any active setting change actions initiated via SetSetting().
	/// </summary>
	public void SaveSettings() {
		SettingsFile.Save(settingsPath);
		foreach (var (setting, section) in actionQueue) {
			if (settingActionDict.ContainsKey(setting)) {
				settingActionDict[setting].Call(SettingsFile.GetValue(section, setting));
			}
		}
		actionQueue.Clear();
	}

	private void WindowMode(string mode) {
		mode = mode.ToLower();
		
		switch (mode) {
			case "fullscreen":
				ProjectSettings.SetSetting("display/window/size/mode", (int)DisplayServer.WindowMode.Fullscreen);
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
				break;
			case "windowed":
				ProjectSettings.SetSetting("display/window/size/mode", (int)DisplayServer.WindowMode.Windowed);
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
				DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
				break;
			case "borderless":
				ProjectSettings.SetSetting("display/window/size/mode", (int)DisplayServer.WindowMode.Maximized);
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Maximized);
				DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true);
				break;
			default:
				GD.PrintErr($"[Settings.cs] Window mode {mode} not recognized.");
				break;
			}
	}

	private void Resolution(string res) {
		res = res.ToLower();
		res = res.Replace("Res_", "");
		string[] strings = res.Split("x");
		int width = strings[0].ToInt();
		int height = strings[1].ToInt();
		ProjectSettings.SetSetting("display/window/size/viewport_width", width);
		ProjectSettings.SetSetting("display/window/size/viewport_height", height);
		DisplayServer.WindowSetSize(new Vector2I(width, height));
	}

	private void VSync(bool mode) {
		ProjectSettings.SetSetting("display/window/vsync/vsync_mode", mode);
		ProjectSettings.SetSetting("application/run/delta_smoothing", mode);
		Engine.MaxFps = Engine.MaxFps; // force engine refresh

	}

	private void MaxFPS(int fps) {
		ProjectSettings.SetSetting("application/run/max_fps", fps);
		Engine.MaxFps = fps;
	}
}