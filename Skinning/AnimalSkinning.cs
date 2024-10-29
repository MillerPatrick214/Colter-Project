using Godot;
using System;

public partial class AnimalSkinning : Node2D
{
	Vector2 DefaultPosition = new Vector2(1110, 589); //I am worred that this will get warped on screen changed. I'm about 100% sure it will tbh. I don't know enough about how ratio changes will adjust this, so for the time being, it will stay as is.
	
	// Called when the node enters the scene tree for the first time.
	KnifeArea KnifeAreaNode;

	bool isMouseOnKnife;
	bool isKnifeHeld;

	Sprite2D BowieKnife;
 	Vector2 mousePos;
	Vector2 rotationAngle;
	public override void _Ready()
	{
		BowieKnife = GetNodeOrNull<Sprite2D>("BowieKnife");
		KnifeAreaNode = GetNodeOrNull<KnifeArea>("BowieKnife/Knife Area");

		if (KnifeAreaNode == null) {
			GD.Print("Animal Skinning Node: Knife Area child node returned null");
		}

		KnifeAreaNode.MouseOnKnife += (isTrue) => isMouseOnKnife = isTrue;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		mousePos = GetGlobalMousePosition();

		if (isMouseOnKnife) {
			if (Input.IsActionJustPressed("UseItem")) {
				GD.Print("UseItem Pressed on Knife");
				isKnifeHeld = true;
			}
			if(Input.IsActionJustPressed("Aim")){
				GD.Print("Dropped Knife");
				isKnifeHeld = false;
			}
		}

		if (isKnifeHeld) {
			BowieKnife.Position = mousePos; 

			float targetAngle = rotationAngle.Angle();
			BowieKnife.Rotation = Mathf.LerpAngle(BowieKnife.Rotation, targetAngle, 0.1f);
		}

		else if (!isKnifeHeld && BowieKnife.Position != DefaultPosition) {
			SheatheKnife();
		}

	}

	public override void _Input(InputEvent @event)  {
		if (isKnifeHeld && @event is InputEventMouseMotion mouseMotion) {
			rotationAngle = mouseMotion.Relative;
			//BowieKnife.Rotation = rotationAngle.Angle();
		}

	}

	public void SheatheKnife() {
			float lerpFactor = .05f;
			
			BowieKnife.Rotation = Mathf.LerpAngle(BowieKnife.Rotation, 0, lerpFactor);
			BowieKnife.Position = BowieKnife.Position.Lerp(DefaultPosition, lerpFactor);

			if (BowieKnife.Position.DistanceTo(DefaultPosition) < .5f) {
        		BowieKnife.Position = DefaultPosition; 					// Snap to position to avoid overshooting
			}
	}


	public void BeginSkinning() { 
	}


}
	// If knife is not on body, no cut no change
	// If knife is on start pos: 
	//	if cut																	cut should be a toggle - once you start you're in. 
	// 		If knife is on wrong area
	//  		detract from quality * time spent cutting on on wrong area
	// 
	//		if knife is on right area
	// 			draw cutline
	//		if knife is !on right || !on wrong
	//			pass
	//
	//		if in end box
	//			end s
	// maybe have a start and end box on ends 
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//
	//