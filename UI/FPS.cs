using Godot;
using System;

public partial class FPS : Control
{
	// Called when the node enters the scene tree for the first time.
	Label labelNode;
	public override void _Ready()
	{
		labelNode = GetNodeOrNull<Label>("Label");
		if (labelNode == null) {
			GD.Print("FPS Node: returned null for child node");
		}
		
		else {
			GD.Print("Ladies and gentlemen... We got him...");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		labelNode.Text = $"FPS: {Engine.GetFramesPerSecond()}";
	}
}
