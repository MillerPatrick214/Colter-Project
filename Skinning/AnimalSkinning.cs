using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;


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

	RichTextLabel SkinComment;

	Line2D CutLine;
	Timer timer;
	
	float MaxLength;
	float SegmentLength; 

	int LineIndex;
	float devAccum;

	Godot.Vector2 DefaultBowiePosition;
	Godot.Vector2 DefaultSheathePosition;

	Dictionary<int, string> DictRating = new Dictionary<int, string> 
	{
		{800, "Perfect!"},
		{1750, "Good!"},
		{2500, "Decent"},
		{3250, "Poor..."},
		{4000, "Shite!"}
	}; 

	
	public override void _Ready()
	{
		BowieKnife = GetNodeOrNull<TextureRect>("BowieKnife");
		KnifeAreaNode = GetNodeOrNull<KnifeArea>("BowieKnife/Knife Area");
		skinningfact = GetNodeOrNull<SkinningFactory>("Skinning Factory"); 
		timer = GetNodeOrNull<Timer>("Timer");
		if (timer == null) {
			GD.PrintErr("Timer Returned null in Animal Skinning");
		}

		Skinnable currSkinnable = null;
		Sheathe = GetNodeOrNull<TextureRect>("Sheathe");
		CutLine = GetNodeOrNull<Line2D>("CutLine");
		SkinComment = GetNodeOrNull<RichTextLabel>("SkinCommentBox/SkinComment");

		if (CutLine == null) {
			GD.Print("Animal Skinning Node: Unable to connect to cutline node");
		}

		if (KnifeAreaNode == null) {
			GD.Print("Animal Skinning Node: Knife Area child node returned null");
		}

		KnifeAreaNode.MouseOnKnife += (isTrue) => isMouseOnKnife = isTrue; //
		skinningfact.SkinningInstance += (instance) => setSkinnable(instance); //connects signal from skinnable object to recieve skinnable function.
		LineIndex = 0;
		devAccum = 0;

		timer.Timeout +=  ResetSkinning;

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
					Vector2 position = BowieKnife.Position;
					
					BowieKnife.Position = new Vector2((float)Mathf.Lerp(position.X, offsetXPosition, 5 * delta), DefaultBowiePosition.Y);

					if (BowieKnife.Position.X - offsetXPosition < 0.1f) {
						BowieKnife.Position = new Vector2((float)offsetXPosition, DefaultBowiePosition.Y);
					}
				}

				if (Input.IsActionJustPressed("UseItem") && !isKnifeHeld) {
					GD.Print("UseItem Pressed on Knife");
					isKnifeHeld = true;
				}
			}

			else if (!isMouseOnKnife && BowieKnife.Position != DefaultBowiePosition) {
				Vector2 position = BowieKnife.Position; 
				BowieKnife.Position = new Vector2((float)Mathf.Lerp(position.X, DefaultBowiePosition.X , 5 * delta), DefaultBowiePosition.Y);
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

			float lerpFactor = .01f;
			
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

	public void Skinning() {
												//Accumulates deviation on the x axis relative to the starting point x
		Godot.Vector2 lastPoint = CutLine.GetPointPosition(LineIndex);
		float distance = lastPoint.DistanceTo(mousePos);

		while (distance >= SegmentLength) {						//while the distance between the last point and the mouse is cucked, we split up the line into iterations of proper segment length.
			float segments = distance / SegmentLength;			//Calculate number of segments we need

			for (int i = 1; i <= segments; i++ ) {				//For each segment we go i/segment length of the way before placing a new point. 							
				Godot.Vector2 newPoint = lastPoint.Lerp(mousePos, i / segments);

				if (CutLine.GetPointPosition(0).DistanceTo(newPoint) >= MaxLength || LineIndex >= 99) {
					CutLine.AddPoint(new Godot.Vector2(newPoint.X, CutLine.GetPointPosition(0).Y + MaxLength));
					RateSkinning(devAccum);
					return;
				}

				CutLine.AddPoint(newPoint);
				devAccum += Mathf.Abs(newPoint.X - currSkinnable.StartMaker.GlobalPosition.X);
				GD.PrintErr($"AnimalSkinning -- devAccum showing {devAccum}");
				LineIndex += 1; 
			}

			lastPoint  = CutLine.GetPointPosition(LineIndex);
			distance = lastPoint.DistanceTo(mousePos);
		}
	}

	public void RateSkinning(float devAccum) 
	{
		foreach (KeyValuePair<int, string> kvp in DictRating) {
			if (devAccum < kvp.Key) {
				SkinComment.Text  = kvp.Value;
				timer.Start(2);	//starts timer that once finished will clear skinning node and text.
				return;
			}
		}
		
		SkinComment.Text  = "Completely Ruined";
		timer.Start(2);
	}

	public void ResetSkinning() 
	{
		SkinComment.Text = "";
		CutLine.ClearPoints();
		isSkinning = false;
		LineIndex = 0;
		devAccum = 0;
		currSkinnable.QueueFree();
	}

	public void BeginSkinning() {
		if (!isSkinning) {
			currSkinnable.PullRectangle();					
			Vector2 rectSize = currSkinnable.GetSize();
			Vector2 rectLocation= currSkinnable.GetShapeLocation();
			Vector2 offset =  currSkinnable.Position;
			Vector2 startPos = currSkinnable.StartMaker.GlobalPosition;

			rectLocation += offset;
			
			GD.Print($"RectangleSize: {rectSize}\nRectangleLocation: {rectLocation}\nstartPOS: {startPos}");

			MaxLength = (rectSize.Y - rectLocation.Y) * .66f; 	// so here I'm saying the total length of the cut line will be 75% of the size of the total collision shape. RectSize.Y is greater(but further down visually) than the location of Y. The difference is the length overall in pixels I beleive.					
			SegmentLength = MaxLength/100; 						// so assuming 100 segements will make up our line

			BowieKnife.Position = startPos;
			GetViewport().WarpMouse(startPos);						
			isSkinning = true;
			CutLine.AddPoint(startPos);
		}
	}
}