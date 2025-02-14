#if TOOLS
using Godot;

[Tool]
public partial class LevelNode : EditorPlugin
{
	public override void _EnterTree()
	{
		Script script = GD.Load<Script>("res://addons/level/Level.cs");
		Texture2D tex = GD.Load<Texture2D>("res://addons/level/Level.png");
		AddCustomType("Level", "Node3D", script, tex);
	}

	public override void _ExitTree()
	{
		RemoveCustomType("Level");
	}
}
#endif
