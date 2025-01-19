using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

public partial class AnimalSkinning : Control
{ 

	KnifeArea KnifeAreaNode;

	bool isMouseOnKnife;
	bool isKnifeOnSkin;
	bool isKnifeHeld;

	bool isSkinning;

	TextureRect BowieKnife;
 	Godot.Vector2 mousePos;
	Godot.Vector2 rotationAngle;

	SkinningFactory skinningfact;
	Skinnable currSkinnable;
	TextureRect Sheathe;

	Line2D CutLine;
	
	float totalLength;
	float SegmentLength; 

	int LineIndex;
	Godot.Vector2 DefaultBowiePosition;
	Godot.Vector2 DefaultSheathePosition;

	public override void _Ready()
	{
		BowieKnife = GetNodeOrNull<TextureRect>("BowieKnife");
		KnifeAreaNode = GetNodeOrNull<KnifeArea>("BowieKnife/Knife Area");
		skinningfact = GetNodeOrNull<SkinningFactory>("Skinning Factory"); 
		Skinnable currSkinnable = null;
		Sheathe = GetNodeOrNull<TextureRect>("Sheathe");
		CutLine = GetNodeOrNull<Line2D>("CutLine");

		if (CutLine == null) {
			GD.Print("Animal Skinning Node: Unable to connect to cutline node");
		}

		if (KnifeAreaNode == null) {
			GD.Print("Animal Skinning Node: Knife Area child node returned null");
		}

		KnifeAreaNode.MouseOnKnife += (isTrue) => isMouseOnKnife = isTrue; //
		skinningfact.SkinningInstance += (instance) => setSkinnable(instance); //connects signal from skinnable object to recieve skinnable function.
		//GetViewport().SizeChanged += setBowieSheathePosition;	 FIXME implement me (pull from old github) I got deleted cause PM fucked up
		//GetViewport().Ready += setBowieSheathePosition;
		LineIndex = 0;

		DefaultBowiePosition = BowieKnife.Position;
		DefaultSheathePosition = Sheathe.Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		mousePos = GetGlobalMousePosition();

		if (isSkinning) {
			Skinning();
		 }

		if (!isKnifeHeld) {
			if (isMouseOnKnife) {
				double offsetXPosition = DefaultBowiePosition.X - 200.0;

				if (BowieKnife.Position.X != offsetXPosition) {
					Godot.Vector2 position = BowieKnife.Position;
					
					BowieKnife.Position = new Godot.Vector2((float)Mathf.Lerp(position.X, offsetXPosition, 5 * delta), DefaultBowiePosition.Y);

					if (BowieKnife.Position.X - offsetXPosition < 0.1f) {
						BowieKnife.Position = new Godot.Vector2((float)offsetXPosition, DefaultBowiePosition.Y);
					}
				}

				if (Input.IsActionJustPressed("UseItem") && !isKnifeHeld) {
					GD.Print("UseItem Pressed on Knife");
					isKnifeHeld = true;
				}
			}

			else if (!isMouseOnKnife && BowieKnife.Position != DefaultBowiePosition) {
				Godot.Vector2 position = BowieKnife.Position; 
				BowieKnife.Position = new Godot.Vector2((float)Mathf.Lerp(position.X, DefaultBowiePosition.X , 5 * delta), DefaultBowiePosition.Y);
				if (DefaultBowiePosition.X - BowieKnife.Position.X < 0.1f) {
						BowieKnife.Position = DefaultBowiePosition;
					}
			}
		}

		if (isKnifeHeld) {
			BowieKnife.Position = mousePos - BowieKnife.PivotOffset; 
			float targetAngle = rotationAngle.Angle();
			BowieKnife.Rotation = Mathf.LerpAngle(BowieKnife.Rotation, targetAngle, 0.1f);
			if(Input.IsActionJustPressed("Aim")){
				GD.Print("Dropped Knife");
				isKnifeHeld = false;
			}

			if (Input.IsActionJustPressed("UseItem") && currSkinnable != null && isKnifeOnSkin) {
				BeginSkinning();
			}
		}

		else if (!isKnifeHeld && BowieKnife.Position != Sheathe.Position && !isSkinning) {
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
		Godot.Vector2 DefaultPosition = Sheathe.Position;

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