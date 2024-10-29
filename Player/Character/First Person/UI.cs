using Godot;
using System;

public partial class UI : Control
{
	// Called when the node enters the scene tree for the first time.

	GodotObject LookingAtObj;
	InteractText InteractTextNode;
	public override void _Ready()
	{
		InteractTextNode = GetNodeOrNull<InteractText>("InteractText");
		if (InteractTextNode == null) {GD.Print("UI Node: Can't return InteractText Node:");}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	
	public override void _Process(double delta)
	{
	}

	public void SetInteractable(GodotObject obj) {
		LookingAtObj = obj;
		TellChild(LookingAtObj);
	}

	public void TellChild(GodotObject newObject) {
		InteractTextNode.Seen(newObject);
	}
}
