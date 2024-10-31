using Godot;
using System;
// This factory node is where we will be instantiating, as a child, the correct skinning scene


public partial class SkinningFactory : Node2D
{ 
	[Signal]
	public delegate void SkinningInstanceEventHandler(Skinnable instance); // Signal to emit when a Skinnable instance is created

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SkinningInstantiate(string filePath) { // here the parent (character) will invoke the function with the appropriate string.
		var scene = GD.Load<PackedScene>(filePath); // load scene
		var instance = scene.Instantiate();			//instantiate scene
		AddChild(instance);							//add as a child
		EmitSignal(SignalName.SkinningInstance, instance);	// emit signal w/ instance  

	}

}
