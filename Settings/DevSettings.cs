using Godot;

[GlobalClass] // appears in Godot's "New Resource" dialog
public partial class DevSettings : Resource
{
	// Edit these values by accessing DevSettings.tres in the Godot inspector


	private static DevSettings _instance;

	public static DevSettings Instance => _instance ??= ResourceLoader.Load<DevSettings>("res://Settings/DevSettings.tres") ?? new DevSettings();
}
