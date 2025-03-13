using Godot;
// This factory node is where we will be instantiating, as a child, the correct skinning scene


public partial class SkinningFactory : Control
{ 
	AspectRatioContainer SkinContainer;

	[Signal]
	public delegate void SkinningInstanceEventHandler(Skinnable instance); // Signal to emit when a Skinnable instance is created

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Events.Instance.BeginSkinning += (SkinningScene) => SkinningInstantiate(SkinningScene);
		SkinContainer = GetNodeOrNull<AspectRatioContainer>("RatioSkinContainer");
		if (SkinContainer == null) {
			GD.Print("SkinningFactory: RatioSkinContainer returned null during connection process");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void SkinningInstantiate(PackedScene scene) { 		// here the parent (character) will invoke the function with the appropriate string.
	
		if (SkinContainer.GetChildren().Count  == 0)	             	// this is kinda cheesy. We really should have a more robust system than this.
		{
			var instance = scene.Instantiate();			 		//instantiate scene
			SkinContainer.AddChild(instance);					//add as a child
			EmitSignal(SignalName.SkinningInstance, instance);	// emit signal w/ instance
			
		}
	}

	
}
