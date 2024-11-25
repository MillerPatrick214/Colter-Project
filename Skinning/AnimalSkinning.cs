using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

public partial class AnimalSkinning : Node2D
{ 

	KnifeArea KnifeAreaNode;

	bool isMouseOnKnife;
	bool isKnifeOnSkin;
	bool isKnifeHeld;

	bool isSkinning;

	Sprite2D BowieKnife;
 	Godot.Vector2 mousePos;
	Godot.Vector2 rotationAngle;

	SkinningFactory skinningfact;
	Skinnable currSkinnable;
	Sprite2D sheathe;

	Line2D CutLine;

	float totalLength;

	float SegmentLength; 

	int LineIndex;


	public override void _Ready()
	{
		BowieKnife = GetNodeOrNull<Sprite2D>("BowieKnife");
		KnifeAreaNode = GetNodeOrNull<KnifeArea>("BowieKnife/Knife Area");
		skinningfact = GetNodeOrNull<SkinningFactory>("Skinning Factory"); 
		Skinnable currSkinnable = null;
		sheathe = GetNodeOrNull<Sprite2D>("Sheathe");
		CutLine = GetNodeOrNull<Line2D>("CutLine");

		if (KnifeAreaNode == null) {
			GD.Print("Animal Skinning Node: Knife Area child node returned null");
		}

		KnifeAreaNode.MouseOnKnife += (isTrue) => isMouseOnKnife = isTrue; //
		skinningfact.SkinningInstance += (instance) => setSkinnable(instance); //connects signal from skinnable object to recieve skinnable function.
		
		LineIndex = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		mousePos = GetGlobalMousePosition();

		if (isSkinning) {
			Skinning();
		 }

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

			if (Input.IsActionJustPressed("UseItem") && currSkinnable != null && isKnifeOnSkin) {
				BeginSkinning();
			}
		}

		else if (!isKnifeHeld && BowieKnife.Position != sheathe.Position && !isSkinning) {
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
		Godot.Vector2 DefaultPosition = sheathe.Position;

			float lerpFactor = .05f;
			
			BowieKnife.Rotation = Mathf.LerpAngle(BowieKnife.Rotation, 0, lerpFactor);
			BowieKnife.Position = BowieKnife.Position.Lerp(DefaultPosition, lerpFactor);

			if (BowieKnife.Position.DistanceTo(DefaultPosition) < .5f) {
        		BowieKnife.Position = DefaultPosition; 					// Snap to position to avoid overshooting
			}
	}

	public void setSkinnable(Skinnable instance) {
		currSkinnable = instance;
		if (currSkinnable != null) {
			currSkinnable.MouseOnSkin += (isTrue) => isKnifeOnSkin = isTrue;
		
		}
	}

	public void Skinning() {			// need to lock bowie knife to skin here too
		 	// so here I'm saying the total length of the cut line will be 75% of the size of the total collision shape. RectSize.Y is greater(but further down visually) than the location of Y. The difference is the length overall in pixels I beleive.					


		float currLength = (float)Math.Sqrt(Math.Pow(mousePos.X - CutLine.GetPointPosition(LineIndex).X, 2) + Math.Pow(mousePos.Y - CutLine.GetPointPosition(LineIndex).Y, 2)); //cur length from the previous point to the mouse pointer.
		
		if (currLength >= SegmentLength) {
			CutLine.AddPoint(mousePos);
			LineIndex += 1;					//add another point to the line index counter.
		}

		if (LineIndex == 99)				//this will need to be changed if we change the num of segements we want
		{
			currSkinnable = null;
			isSkinning = false;
			LineIndex = 0;
		}
	}

	public void BeginSkinning() {
		if (!isSkinning) {
			currSkinnable.PullRectangle();					
			Godot.Vector2 rectSize = currSkinnable.GetSize();
			Godot.Vector2 rectLocation= currSkinnable.GetShapeLocation();
			Godot.Vector2 offset =  currSkinnable.Position;
			Godot.Vector2 startPos = currSkinnable.StartMaker.GlobalPosition;

			rectLocation += offset;
			
			GD.Print($"RectangleSize: {rectSize}\nRectangleLocation: {rectLocation}\nstartPOS: {startPos}");

			totalLength = (rectSize.Y - rectLocation.Y) * .75f; 	// so here I'm saying the total length of the cut line will be 75% of the size of the total collision shape. RectSize.Y is greater(but further down visually) than the location of Y. The difference is the length overall in pixels I beleive.					
			SegmentLength = totalLength/100;						// so assuming 100 segements will make up our line

			BowieKnife.Position = startPos;
			GetViewport().WarpMouse(startPos);						//FIX ME -- Love this visually. Just need to fix the line drawing during the warp FIXED but I forget how lmao
			isSkinning = true;
			CutLine.AddPoint(startPos);
		}
	}

}
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