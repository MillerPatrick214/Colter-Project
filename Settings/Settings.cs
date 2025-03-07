using Godot;

public partial class Settings : Node
{
	public static Settings Instance { get; private set; }

	[Signal] public delegate void SettingChangedEventHandler();
	[Signal] public delegate void SettingUnchangedEventHandler();

	private const string defaultPath = "res://Settings/DefaultSettings.cfg";
	private ConfigFile defaultConfig = new ConfigFile();
	private const string settingsPath = "user://settings/settings.cfg";
	private ConfigFile SettingsConfig = new ConfigFile();
	private ConfigFile settingsConfigFile = new ConfigFile();
	private Godot.Collections.Dictionary<string, Callable>  settingActionDict = new();
	private Godot.Collections.Dictionary<string, string> actionQueue = new();

	public readonly Godot.Collections.Array<string> WindowModes = new() {
		"fullscreen", "windowed", "borderless"
	};
	public readonly Godot.Collections.Array<string> Resolutions = new () {
		"3840x2160", "3440x1440", "2560x1600", "2560x1440", "1920x1080", "1920x1200", "1366x768"
	};
	public readonly Godot.Collections.Array<int> FPSs = new() {
		0, 30, 60, 120, 144, 240
	};

	public override void _Ready() {
        Instance ??= this;
        if (Instance != this) QueueFree();

        defaultConfig.Load(defaultPath);

        if (SettingsConfig.Load(settingsPath) != Error.Ok) {
            if (!FileAccess.FileExists(settingsPath)) {
                GD.Print($"[color=red]Settings: [/color] No file found at {settingsPath}");
                GD.Print($"[color=red]Settings: [/color] Generating default settings.");

                string absDir = ProjectSettings.GlobalizePath(settingsPath.GetBaseDir());
                Error dirErr = DirAccess.MakeDirAbsolute(absDir);
                if (dirErr != Error.Ok) { GD.PrintErr($"Error creating directory: {absDir}"); }
                FileAccess.Open(settingsPath, FileAccess.ModeFlags.WriteRead);

                defaultConfig.Save(settingsPath);
                SettingsConfig.Load(settingsPath);
            }
            else { GD.PrintErr($"Can't load ConfigFile at {settingsPath}"); }
        }
        settingsConfigFile.Load(settingsPath);

        settingActionDict = new() {
            // [gameplay]
            // [graphics]
            {"window_mode", new Callable(this, MethodName.WindowMode)},
            {"resolution", new Callable(this, MethodName.Resolution)},
            {"vsync", new Callable(this, MethodName.VSync)},
            {"max_fps", new Callable(this, MethodName.MaxFPS)},
            // [audio]
            // {"master_volume", new Callable(this, MethodName.MasterVolumne)}
        };

        LoadDefaults();
    }

	private void LoadDefaults() {
		// loop through default file and check the settings file has all settings
		foreach (string dSection in defaultConfig.GetSections()) {
			foreach (string dKey in defaultConfig.GetSectionKeys(dSection)) {
				if (!SettingsConfig.HasSectionKey(dSection, dKey)) {
					SetSetting(dSection, dKey, defaultConfig.GetValue(dSection, dKey));
				}
			}
		}
		// loop through settings file and remove any not in the default
		foreach (string section in SettingsConfig.GetSections()) {
			foreach (string key in SettingsConfig.GetSectionKeys(section)) {
				if (!defaultConfig.HasSectionKey(section, key)) {
					SettingsConfig.EraseSectionKey(section, key);
				}
			}
		}
		SaveSettings();
	}

	/// <summary>
	/// Gets a setting value from the active ConfigFile in-memory object.
	/// </summary>
	/// <param name="section">The settings.ini section string.</param>
	/// <param name="key">The settings.ini key string.</param>
	/// /// <param name="file">Optional. If set to true, pulls settings from file instead of object. Default is false.</param>
	/// <returns>The setting value.</returns>
	public Variant GetSetting(string section, string key, bool file = false) {
		if (file == false) { return SettingsConfig.GetValue(section, key); }
		return settingsConfigFile.GetValue(section, key);
	}

	/// <summary>
	/// Updates a settings value to the ConfigFile in-memory object and adds corresponding save action to queue. 
	/// </summary>
	/// <param name="section">The settings.ini section string.</param>
	/// <param name="key">The settings.ini key string.</param>
	/// <param name="value">The settings value to be applied.</param>
	public void SetSetting(string section, string key, Variant value) {
		section = section.ToLower();
		key = key.ToLower();

		if (value.ToString().ToLower() == GetSetting(section, key, true).ToString().ToLower()) { 
			if (actionQueue.ContainsKey(key)) { actionQueue.Remove(key); }
			if (actionQueue.Count == 0) { EmitSignal(SignalName.SettingUnchanged); }
			return;
		}
		SettingsConfig.SetValue(section, key, value);
		actionQueue[key] = section;
		EmitSignal(SignalName.SettingChanged);
	}

	/// <summary>
	/// Saves active configfile in-memory object to file and runs any active setting change actions initiated via SetSetting().
	/// </summary>
	public void SaveSettings() {
		SettingsConfig.Save(settingsPath);
		SettingsConfig.Load(settingsPath);
		settingsConfigFile.Load(settingsPath);
		foreach (var (setting, section) in actionQueue) {
			if (settingActionDict.ContainsKey(setting)) {
				settingActionDict[setting].Call(SettingsConfig.GetValue(section, setting));
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