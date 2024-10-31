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




}	//
	// 	I think the simplest way to imagine this as laying down rope from the starting position, after a certain length of travel while laying it down, we look at the y position for the "quality" only the straighest line directly down from start will have the greatest y position. 
	//	Everything else/every twist and turn will take somthing off of the y pos.
	//
	//	We can have a total length value supplied by the deerskintest node. This would likely have to scale with the viewport though. Anothe way to measure it might be by looking at the y length of the sprite currently * .75
	//
	// 
	// if knife on body and click
	//		lerp to start position
	//		change sprite to cut
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