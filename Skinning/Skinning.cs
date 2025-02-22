using Godot;
using static FurInvItem;

public partial class Skinning : Control
{ 
	[ExportCategory("Line Trace Scoring Settings")]
	[Export] public float SkinningDistanceWeight;
	[Export] public float SkinningDeviationWeight;
	[Export] public float SkinningReversalWeight;
	[Export] public float SkinningJitterWeight;

	[ExportCategory("Cut Line Metrics")]
	[Export] float MaxLength;
	[Export] float Segments;

	KnifeArea KnifeAreaNode;
	bool isMouseOnKnife;
	bool isKnifeOnSkin;
	bool isKnifeHeld;
	bool isSkinning;

	TextureRect BowieKnife;

 	Vector2 mousePos;
	Vector2 rotationAngle;

	SkinningFactory skinningfact;
	Skinnable currSkinnable;
	TextureRect Sheathe;

	RichTextLabel SkinComment;

	Line2D CutLine;
	Timer timer;
	float SegmentLength;
	int LineIndex;
	float devAccum;
	float cutLength ;
	float revAccum ;
	float jitterAccum;

	float offsetXPosition;
	Vector2 offsetPosition;
	Vector2 DefaultBowiePosition;
	Vector2 DefaultSheathePosition;

	public override void _Ready()
	{
		Hide();

		skinningfact = GetNodeOrNull<SkinningFactory>("Skinning Factory"); 
		timer = GetNodeOrNull<Timer>("Timer");
		Sheathe = GetNodeOrNull<TextureRect>("Sheathe");
		CutLine = GetNodeOrNull<Line2D>("CutLine");
		SkinComment = GetNodeOrNull<RichTextLabel>("SkinCommentBox/SkinComment");
		BowieKnife = GetNodeOrNull<TextureRect>("BowieKnife");
		KnifeAreaNode = GetNodeOrNull<KnifeArea>("BowieKnife/Knife Area");
		
		if (BowieKnife == null || KnifeAreaNode == null || skinningfact == null || timer == null)
		{
			GD.PrintErr("Error in AnimalSkinning: One of the Nodes returned null");
			GD.PrintErr(BowieKnife == null ? "BowieKnife is null" : "");
			GD.PrintErr(KnifeAreaNode == null ? "KnifeAreaNode is null" : "");
			GD.PrintErr(skinningfact == null ? "skinningfact is null" : "");
			GD.PrintErr(timer == null ? "timer is null" : "");
			GD.PrintErr(Sheathe == null ? "Sheate is null" : "");
		}
		
		if (timer == null) {
			GD.PrintErr("Timer Returned null in Animal Skinning");
		}

		if (CutLine == null) {
			GD.Print("Animal Skinning Node: Unable to connect to cutline node");
		}

		if (KnifeAreaNode == null) {
			GD.Print("Animal Skinning Node: Knife Area child node returned null");
		}
		isMouseOnKnife = false;

		KnifeAreaNode.MouseOnKnife += (isTrue) => isMouseOnKnife = isTrue; //
		skinningfact.SkinningInstance += (instance) => SetSkinnable(ref instance); //connects signal from skinnable object to recieve skinnable function.

		timer.WaitTime = 2.0f;
		timer.Timeout += () => ResetSkinning();

		DefaultBowiePosition = BowieKnife.Position;
		DefaultSheathePosition = Sheathe.Position;

		offsetXPosition = DefaultBowiePosition.X - 200.0f;
		offsetPosition = new Vector2(offsetXPosition, DefaultBowiePosition.Y);
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		mousePos = GetGlobalMousePosition();

		GD.Print($"isSkinning: {isSkinning}");

		if (isSkinning) {
			SkinningEvent();
		}

		if (!isKnifeHeld) {
			if (isMouseOnKnife && BowieKnife.Position <= DefaultBowiePosition && BowieKnife.Position >= offsetPosition) {

				if (BowieKnife.Position.X != offsetXPosition) {
					Vector2 position = BowieKnife.Position;
					
					BowieKnife.Position = new Vector2((float)Mathf.Lerp(position.X, offsetXPosition, 5 * delta), DefaultBowiePosition.Y);

					if (BowieKnife.Position.X - offsetXPosition < 0.1f) {
						BowieKnife.Position = new Vector2((float)offsetXPosition, DefaultBowiePosition.Y);
					}
				}

				if (Input.IsActionJustPressed("UseItem") && !isKnifeHeld) {
					//GD.Print("UseItem Pressed on Knife");
					isKnifeHeld = true;
				}
			}

			else if (!isMouseOnKnife && BowieKnife.Position != DefaultBowiePosition) {
				Vector2 position = BowieKnife.Position; 
				BowieKnife.Position = new Vector2((float)Mathf.Lerp(position.X, DefaultBowiePosition.X , 3 * delta), DefaultBowiePosition.Y);
				if (DefaultBowiePosition.X - BowieKnife.Position.X < 0.5f) {
						BowieKnife.Position = DefaultBowiePosition;
					}
			}
		}

		if (isKnifeHeld) {
			BowieKnife.Position = mousePos - BowieKnife.PivotOffset; 
			float targetAngle = rotationAngle.Angle();
			BowieKnife.Rotation = Mathf.LerpAngle(BowieKnife.Rotation, targetAngle, 0.1f);
			if(Input.IsActionJustPressed("Aim")){
				//GD.Print("Dropped Knife");
				isKnifeHeld = false;
			}

			if (Input.IsActionJustPressed("UseItem") && currSkinnable != null && isKnifeOnSkin) {
				BeginSkinning();
			}
		}	

		else if (!isKnifeHeld && (BowieKnife.Position != DefaultBowiePosition || BowieKnife.Rotation != 0.0f)) {
			SheatheKnife(delta);
		}
	}

	public override void _Input(InputEvent @event)  {
		if (isKnifeHeld && @event is InputEventMouseMotion mouseMotion) {
			rotationAngle = mouseMotion.Relative;
			//BowieKnife.Rotation = rotationAngle.Angle();
		}
	}

	public void SheatheKnife(double delta)
	{
		Vector2 DefaultPosition = Sheathe.Position;
			float lerpFactor = (float)delta * 2.0f;
			
			BowieKnife.Rotation = Mathf.LerpAngle(BowieKnife.Rotation, 0, lerpFactor);
			BowieKnife.Position = BowieKnife.Position.Lerp(DefaultBowiePosition, lerpFactor);

			if (BowieKnife.Position.DistanceTo(DefaultBowiePosition) < 2.0f) 
			{
        		BowieKnife.Position = DefaultBowiePosition; 					// Snap to position to avoid overshooting
			}

			if (Mathf.Abs(BowieKnife.Rotation) < .1f) {
				BowieKnife.Rotation = 0;
			}
	}

	public void SetSkinnable(ref Skinnable instance) {
			currSkinnable = instance;
		if (currSkinnable != null) {
			currSkinnable.MouseOnSkin += (isTrue) => isKnifeOnSkin = isTrue;
		}
		
	}

	public void SkinningEvent() {
		Vector2 lastPoint = CutLine.GetPointPosition(LineIndex);
		float distance = lastPoint.DistanceTo(mousePos);

		GD.PrintErr(distance);
		GD.PrintErr(SegmentLength);

		// Only iterate while mouse movement per frame (distance) exceeds a single segment length
		while (distance >= SegmentLength) {
			for (int i = 1; i <= Segments; i++ ) {		// For each segment we go i/segment length of the way before placing a new point						
				Vector2 newPoint = lastPoint.Lerp(mousePos, i / Segments);

				// Terminate if the mouse passes the bottom of the skinning area
				if (lastPoint.Y >= currSkinnable.StartMaker.GlobalPosition.Y + MaxLength) {
					// Normalize to ideal cut line length and invert penalties to calculate scores
					float distScore = 1.0f - (Mathf.Abs(cutLength - MaxLength) / MaxLength);
					float devScore = 1.0f - (devAccum / MaxLength);
					float revScore = 1.0f - (revAccum / MaxLength);
					float jitterScore = 1.0f - (jitterAccum / MaxLength);

					// Pass scores to RateSkinning to weight and combine scores to calculate quality
					RateSkinning(distScore, devScore, revScore, jitterScore);
					timer.Start();
					isSkinning = false;

					GD.PrintRich(
						"[color=cyan]Skinning: [/color]Cut Length: " + cutLength + "\n" +
						"[color=cyan]Skinning: [/color]X Deviation Accumulated: " + devAccum + "\n" +
						"[color=cyan]Skinning: [/color]Reversal Penalty Accumulated: " + revAccum + "\n" +
						"[color=cyan]Skinning: [/color]Jitter Penalty Accumulated: " + jitterAccum);

					return;
				}

				CutLine.AddPoint(newPoint);

				// Accumulates distance iterations to form cutLine total length
				cutLength += distance;

				// Accumulates deviation on the x axis relative to the starting point x
				devAccum += Mathf.Abs(newPoint.X - currSkinnable.StartMaker.GlobalPosition.X);
				
				// Accumulates reversal penalty for any upward movements
				if (newPoint.Y >= lastPoint.Y){
					revAccum += Mathf.Abs(newPoint.Y - lastPoint.Y);
				}

				// Accumulates jitteriness penalty by comparing current x deviation and prior x deviation
				if (LineIndex >= 2) {
					float prevDelta = lastPoint.X - CutLine.GetPointPosition(LineIndex - 2).X;
					float currDelta = newPoint.X - lastPoint.X;
					jitterAccum += Mathf.Abs(currDelta - prevDelta);
				}

				LineIndex += 1; 
			}

			lastPoint  = CutLine.GetPointPosition(LineIndex);
			distance = lastPoint.DistanceTo(mousePos);
		}
	}

	public void RateSkinning(float distScore, float devScore, float revScore, float jitterScore) 
	{
		GD.PrintRich(
		"[color=cyan]Skinning: [/color]Distance Score: " + distScore + "\n" +
		"[color=cyan]Skinning: [/color]Deviation Score: " + devScore + "\n" +
		"[color=cyan]Skinning: [/color]Reversal Score: " + revScore + "\n" +
		"[color=cyan]Skinning: [/color]Jitter Score: " + jitterScore);

		Godot.Collections.Dictionary<int, string> DictRatingComment = new()
		{
			{(int)FurQuality.NOTSET, "Note to dev: You didn't set the fur quality dumbass!"},
			{(int)FurQuality.Perfect, "Great skinnin' partner!"},
			{(int)FurQuality.Good, "Ay, not so bad bucko."},
			{(int)FurQuality.Decent, "Eh, could be better...could be worse."},
			{(int)FurQuality.Poor, "Wow, that looks pretty rough."},
			{(int)FurQuality.Shite, "Holy shite, that's the worst skinnin' I ever seen!"}
		};


		float finalScore = Mathf.Clamp(100 * ((SkinningDistanceWeight * distScore) + (SkinningDeviationWeight * devScore) + (SkinningReversalWeight * revScore) + (SkinningJitterWeight * jitterScore)), 0, 100);
		
		GD.PrintRich("[color=cyan]Skinning: [/color][color=green]Final Score: [/color]" + finalScore );

		Godot.Collections.Dictionary<int, float> DictRatingScore = new()
		{
			{(int)FurQuality.Perfect, 90},
			{(int)FurQuality.Good, 70},
			{(int)FurQuality.Decent, 50},
			{(int)FurQuality.Poor, 30},
			{(int)FurQuality.Shite, 10},
			{(int)FurQuality.NOTSET, 0},
		};

		foreach (var (quality, score) in DictRatingScore) {
			if (finalScore >= score) {
				GD.PrintRich("[color=cyan]Skinning: [/color]Final score greater than: " + score + ".\nFur Quality set to:" + quality );
				currSkinnable.FurInvItem.SetQuality(quality);
				Player.Instance.Inventory.PickUpItem(currSkinnable.FurInvItem);
				SkinComment.Text = DictRatingComment[quality];
				timer.Start(2);
				return;
			}
		}	
	}

	public void ResetSkinning() 
	{
		SkinComment.Text = "";
		CutLine.ClearPoints();
		LineIndex = 0;
		devAccum = 0;
		currSkinnable.QueueFree();
		currSkinnable = null;
		GD.PrintRich("[color=cyan]Skinning: [/color]Skinning Reset.");
		
	}

	public void BeginSkinning() {
		if (!isSkinning) {
			GD.PrintRich("[color=cyan]Skinning: [/color]Beginning skinning..");
			currSkinnable.PullRectangle();					
			Vector2 rectSize = currSkinnable.GetSize();
			Vector2 rectLocation= currSkinnable.GetShapeLocation();
			Vector2 offset =  currSkinnable.Position;
			Vector2 startPos = currSkinnable.StartMaker.GlobalPosition;

			rectLocation += offset;
			
			GD.Print($"RectangleSize: {rectSize}\nRectangleLocation: {rectLocation}\nstartPOS: {startPos}");

			MaxLength = (rectSize.Y - rectLocation.Y) * .66f; 	// so here I'm saying the total length of the cut line will be 75% of the size of the total collision shape. RectSize.Y is greater(but further down visually) than the location of Y. The difference is the length overall in pixels I beleive.					
			SegmentLength = MaxLength/Segments; 						// so assuming 100 segements will make up our line

			BowieKnife.Position = startPos;
			GetViewport().WarpMouse(startPos);	
			CutLine.AddPoint(startPos);					
			isSkinning = true;
		}
	}
}