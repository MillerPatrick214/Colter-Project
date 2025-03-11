using Godot;

public partial class LevelManager : Node3D
{

	public static LevelManager Instance { get; private set; } 
	public Node CurrentScene { get; set; }


public void GotoScene(string path)
{
    // This function will usually be called from a signal callback,
    // or some other function from the current scene.
    // Deleting the current scene at this point is
    // a bad idea, because it may still be executing code.
    // This will result in a crash or unexpected behavior.

    // The solution is to defer the load to a later time, when
    // we can be sure that no code from the current scene is running:

    CallDeferred(MethodName.DeferredGotoScene, path);
}

public void DeferredGotoScene(string path)
{
    Player.Instance.SaveData();
    CurrentScene.Free();

    // Load a new scene.
    var nextScene = GD.Load<PackedScene>(path);

    // Instance the new scene.
    CurrentScene = nextScene.Instantiate();

    // Add it to the active scene, as child of root.
    GetTree().Root.AddChild(CurrentScene);

    // Optionally, to make it compatible with the SceneTree.change_scene_to_file() API.
    GetTree().CurrentScene = CurrentScene;
}

    public override void _Ready()
    {
        Instance ??= this;
        if (Instance != this) QueueFree();
		Viewport root = GetTree().Root;
		CurrentScene = root.GetChild(-1);
	}
}

/*

// Add to 'Scene1.cs'.
private void OnButtonPressed()
{
    var global = GetNode<Global>("/root/Global");
    global.GotoScene("res://Scene2.tscn");
}

// Add to 'Scene2.cs'.
private void OnButtonPressed()
{
    var global = GetNode<Global>("/root/Global");
    global.GotoScene("res://Scene1.tscn");
}
*/


