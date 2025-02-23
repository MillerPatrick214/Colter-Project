using System.Linq;
using Godot;
using static FurInvItem;

public partial class Skinning : Control
{
	[ExportCategory("Line Trace Scoring Settings")]
	[Export(PropertyHint.None, "suffix:%")] public float DistanceWeight = 90;
	[Export(PropertyHint.None, "suffix:%")] public float DeviationWeight = 90;
	[Export(PropertyHint.None, "suffix:%")] public float ReversalWeight = 55;
	[Export(PropertyHint.None, "suffix:%")] public float JitterWeight = 55;

	[ExportCategory("Line Trace Score Quality Thresholds")]
	[Export(PropertyHint.None, "suffix:%")] public float PerfectScoreThreshold = 90;
	[Export(PropertyHint.None, "suffix:%")] public float GoodScoreThreshold = 70;
	[Export(PropertyHint.None, "suffix:%")] public float DecentScoreThreshold = 50;
	[Export(PropertyHint.None, "suffix:%")] public float PoorScoreThreshold = 30;
	[Export(PropertyHint.None, "suffix:%")] public float ShiteScoreThreshold = 10;

	[ExportCategory("Cut Line Metrics")]
	[Export(PropertyHint.None, "suffix:%")] public float CutLengthRatio = 66;
	[Export(PropertyHint.None, "suffix:segments")] float Segments = 100.0f;

	[ExportCategory("Skinnable Node")]
	[Export] Skinnable CurrentSkinnable;

	KnifeArea KnifeAreaNode;
	bool isMouseOnKnife = false;
	bool isKnifeOnSkin = false;
	bool isKnifeHeld = false;
	bool isSkinning = false;

	TextureRect BowieKnife;
	Vector2 mousePos;

	Vector2 rotationAngle;
	SkinningFactory skinningfact;
	TextureRect Sheathe;
	RichTextLabel SkinComment;
	Line2D CutLine;
	Timer timer;

	float IdealCutLength;
	float SegmentLength;
	int LineIndex;
	float devAccum;
	float cutLength;
	float revAccum;
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

		if (timer == null)
		{
			GD.PrintErr("Timer Returned null in Animal Skinning");
		}

		if (CutLine == null)
		{
			GD.PrintErr("Animal Skinning Node: Unable to connect to cutline node");
		}

		if (KnifeAreaNode == null)
		{
			GD.PrintErr("Animal Skinning Node: Knife Area child node returned null");
		}

		KnifeAreaNode.MouseOnKnife += (isTrue) => isMouseOnKnife = isTrue;
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

		if (isSkinning)
		{
			SkinningEvent();
		}

		if (!isKnifeHeld)
		{
			if (isMouseOnKnife && BowieKnife.Position <= DefaultBowiePosition && BowieKnife.Position >= offsetPosition)
			{

				if (BowieKnife.Position.X != offsetXPosition)
				{
					Vector2 position = BowieKnife.Position;

					BowieKnife.Position = new Vector2((float)Mathf.Lerp(position.X, offsetXPosition, 5 * delta), DefaultBowiePosition.Y);

					if (BowieKnife.Position.X - offsetXPosition < 0.1f)
					{
						BowieKnife.Position = new Vector2((float)offsetXPosition, DefaultBowiePosition.Y);
					}
				}

				if (Input.IsActionJustPressed("UseItem"))
				{
					//GD.Print("UseItem Pressed on Knife");
					isKnifeHeld = true;
				}
			}

			else if (!isMouseOnKnife && BowieKnife.Position != DefaultBowiePosition)
			{
				Vector2 position = BowieKnife.Position;
				BowieKnife.Position = new Vector2((float)Mathf.Lerp(position.X, DefaultBowiePosition.X, 3 * delta), DefaultBowiePosition.Y);
				if (DefaultBowiePosition.X - BowieKnife.Position.X < 0.5f)
				{
					BowieKnife.Position = DefaultBowiePosition;
				}
			}
		}

		if (isKnifeHeld)
		{
			BowieKnife.Position = mousePos - BowieKnife.PivotOffset;
			float targetAngle = rotationAngle.Angle();
			BowieKnife.Rotation = Mathf.LerpAngle(BowieKnife.Rotation, targetAngle, 0.1f);
			if (Input.IsActionJustPressed("Aim"))
			{
				isKnifeHeld = false;
			}

			if (Input.IsActionJustPressed("UseItem") && CurrentSkinnable != null && isKnifeOnSkin && !isSkinning)
			{
				BeginSkinning();
			}
		}

		else if (!isKnifeHeld && (BowieKnife.Position != DefaultBowiePosition || BowieKnife.Rotation != 0.0f))
		{
			SheatheKnife(delta);
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (isKnifeHeld && @event is InputEventMouseMotion mouseMotion)
		{
			rotationAngle = mouseMotion.Relative;
			//BowieKnife.Rotation = rotationAngle.Angle();
		}
	}

	public void SheatheKnife(double delta)
	{
		float lerpFactor = (float)delta * 2.0f;

		BowieKnife.Rotation = Mathf.LerpAngle(BowieKnife.Rotation, 0, lerpFactor);
		BowieKnife.Position = BowieKnife.Position.Lerp(DefaultBowiePosition, lerpFactor);

		if (BowieKnife.Position.DistanceTo(DefaultBowiePosition) < 2.0f)
		{
			BowieKnife.Position = DefaultBowiePosition;     // Snap to position to avoid overshooting
		}

		if (Mathf.Abs(BowieKnife.Rotation) < .1f)
		{
			BowieKnife.Rotation = 0;
		}
	}

	public void SetSkinnable(ref Skinnable instance)
	{
		CurrentSkinnable = instance;
		if (CurrentSkinnable != null)
		{
			CurrentSkinnable.MouseOnSkin += (isTrue) => isKnifeOnSkin = isTrue;
		}

	}

	public void SkinningEvent()
	{
		Vector2 lastPoint = CutLine.GetPointPosition(LineIndex);
		float distance = lastPoint.DistanceTo(mousePos);

		// Only iterate while mouse movement per frame (distance) exceeds a single segment length
		while (distance >= SegmentLength)
		{
			for (int i = 1; i < Segments; i++ ) {		// For each segment we go i/segment length of the way before placing a new point						
				Vector2 newPoint = lastPoint.Lerp(mousePos, i / Segments);
				CutLine.AddPoint(newPoint);
				LineIndex++;
				
				// Accumulate length of each segment
				cutLength += lastPoint.DistanceTo(newPoint);

				// Accumulate deviation on the x axis relative to the starting point x
				devAccum += Mathf.Abs(newPoint.X - CurrentSkinnable.StartMaker.GlobalPosition.X);

				// Accumulate reversal penalty for any upward movements
				if (newPoint.Y >= lastPoint.Y)
				{
					revAccum += Mathf.Abs(newPoint.Y - lastPoint.Y);
				}

				// Accumulate jitteriness penalty by comparing current x deviation and prior x deviation
				if (LineIndex >= 2)
				{
					float prevDelta = lastPoint.X - CutLine.GetPointPosition(LineIndex - 2).X;
					float currDelta = newPoint.X - lastPoint.X;
					jitterAccum += Mathf.Abs(currDelta - prevDelta);
				}

				// Terminate if the mouse passes the bottom of the skinning area
				if (lastPoint.Y >= CurrentSkinnable.StartMaker.GlobalPosition.Y + IdealCutLength) {

					// Normalize to ideal cut line length and invert penalties to calculate scores
					float distScore = 1.0f - Mathf.Abs(cutLength - IdealCutLength) / IdealCutLength * 0.1f;
					float devScore = 1.0f - devAccum / IdealCutLength * 0.1f;
					float revScore = 1.0f - revAccum / IdealCutLength * 0.1f;
					float jitterScore = 1.0f - jitterAccum / IdealCutLength * 0.1f;

					GD.PrintRich(
					$"[color=cyan]Skinning: [/color]Cut Length: {cutLength}\n" +
					$"[color=cyan]Skinning: [/color]X Deviation Accumulated: {devAccum}\n" +
					$"[color=cyan]Skinning: [/color]Reversal Penalty Accumulated: {revAccum}\n" +
					$"[color=cyan]Skinning: [/color]Jitter Penalty Accumulated: {jitterAccum}");

					// Pass scores to RateSkinning to weight and combine scores to calculate quality
					RateSkinning(distScore, devScore, revScore, jitterScore);
					timer.Start();
					isSkinning = false;

					return;
				}
			}
			// Update distance and LineIndex for next while-loop iteration
			lastPoint = CutLine.GetPointPosition(LineIndex);
			distance = lastPoint.DistanceTo(mousePos);
		}
	}

	public void RateSkinning(float distScore, float devScore, float revScore, float jitterScore)
	{
		GD.PrintRich(
		$"[color=cyan]Skinning: [/color][color=purple]Metrics: [/color]Distance Score: {distScore}\n" +
		$"[color=cyan]Skinning: [/color][color=purple]Metrics: [/color]Deviation Score: {devScore}\n" +
		$"[color=cyan]Skinning: [/color][color=purple]Metrics: [/color]Reversal Score: {revScore}\n" +
		$"[color=cyan]Skinning: [/color][color=purple]Metrics: [/color]Jitter Score: {jitterScore}");

		Godot.Collections.Dictionary<int, string> DictRatingComment = new()
		{
			{(int)FurQuality.NOTSET, "Note to dev: You didn't set the fur quality dumbass!"},
			{(int)FurQuality.Perfect, "Great skinnin' partner!"},
			{(int)FurQuality.Good, "Ay, not so bad bucko."},
			{(int)FurQuality.Decent, "Eh, could be better...could be worse."},
			{(int)FurQuality.Poor, "Wow, that looks pretty rough."},
			{(int)FurQuality.Shite, "Holy shite, that's the worst skinnin' I ever seen!"}
		};

		// Prevent divide by 0 error
		float weightSum = DistanceWeight + DeviationWeight + ReversalWeight + JitterWeight;
		if (weightSum <= 0)
		{
			GD.PrintErr("Skinning metric weights invalid. Setting weightSum to 1.");
			weightSum = 1;
		}

		// Take a weighted average of scores by taking weighted scores and dividing by sum of weights
		float weightedAvg = (DistanceWeight * distScore + 
			DeviationWeight * devScore + 
			ReversalWeight * revScore + 
			JitterWeight * jitterScore) / weightSum;

		// Multiplicative slight penalty for exceptionally low scores
		float penalty = 1.0f - (1.0f - distScore) * (1.0f - devScore) * (1.0f - revScore) * (1.0f - jitterScore);

		// Score 0 - 100 based on weighted average * penalty
		float finalScore = Mathf.Clamp(100 * weightedAvg * penalty, 0, 100);

		GD.PrintRich($"[color=cyan]Skinning: [/color][color=green][color=purple]Metrics: [/color]Final Score: [/color]{finalScore}");

		Godot.Collections.Dictionary<int, float> DictRatingScore = new()
		{
			{(int)FurQuality.NOTSET, 100},
			{(int)FurQuality.Perfect, PerfectScoreThreshold},
			{(int)FurQuality.Good, GoodScoreThreshold},
			{(int)FurQuality.Decent, DecentScoreThreshold},
			{(int)FurQuality.Poor, PoorScoreThreshold},
			{(int)FurQuality.Shite, ShiteScoreThreshold}
		};

		// Iterates the scoring dictionary based on descending score
		foreach (var (quality, score) in DictRatingScore.OrderByDescending(kvp => kvp.Value))
		{
			if (finalScore > DictRatingScore[quality])
			{
				// Assigns corresponding quality before passing a PickUpItem call to the Players inventory for the FurInvItem
				GD.PrintRich($"[color=cyan]Skinning: [/color][color=purple]Metrics: [/color]Fur Quality set to: {quality} (Threshold surpassed: {score})");
				
				CurrentSkinnable.FurInvItem.SetQuality(quality);
				Player.Instance.Inventory.PickUpItem(CurrentSkinnable.FurInvItem);
				SkinComment.Text = DictRatingComment[quality];
				timer.Start(2);
				return;
			}
		}
	}

	public void BeginSkinning()
	{
		if (!isSkinning)
		{
			CurrentSkinnable.PullRectangle();

			Vector2 RectangleSize = CurrentSkinnable.GetSize();
			Vector2 RectangleLocation = CurrentSkinnable.GetShapeLocation();
			Vector2 startPos = CurrentSkinnable.StartMaker.GlobalPosition;

			GD.PrintRich($"[color=cyan]Skinning: [/color][color=blue]Rectangle: [/color]Size: {RectangleSize}");
			GD.PrintRich($"[color=cyan]Skinning: [/color][color=blue]Rectangle: [/color]Location: {RectangleLocation}");
			GD.PrintRich($"[color=cyan]Skinning: [/color]Start Position: {startPos}");

			IdealCutLength = RectangleSize.Y* CutLengthRatio / 100; 					
			SegmentLength = IdealCutLength/ Segments;

			BowieKnife.Position = startPos;
			GetViewport().WarpMouse(startPos);
			CutLine.AddPoint(startPos);

			isSkinning = true;
		}
	}

	public void ResetSkinning()
	{
		// Clear UI text
		SkinComment.Text = "";

		// Reset line tracing data
		CutLine.ClearPoints();
		LineIndex = 0;
		cutLength = 0;
		devAccum = 0;
		revAccum = 0;
		jitterAccum = 0;

		// Reset knife position & state
		isSkinning = false;
		isKnifeHeld = false;
		isKnifeOnSkin = false;
		isMouseOnKnife = false;
		BowieKnife.Position = DefaultBowiePosition;
		BowieKnife.Rotation = 0;

		// Reset Skinnable object
		if (CurrentSkinnable != null)
		{
			CurrentSkinnable.QueueFree();
			CurrentSkinnable = null;
		}

		// Stop any running timers
		timer.Stop();

		GD.PrintRich("[color=cyan]Skinning: [/color]Reset complete.");
	}
}