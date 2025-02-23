using Godot;

public partial class Skinnable : TextureRect 
{
	[Signal]
	public delegate void MouseOnSkinEventHandler(bool isTrue);

	[Export]
	public FurInvItem FurInvItem;

	public Vector2 KnifeStartPosition;
	public Marker2D StartMaker;

	CollisionShape2D CollShape;
	Rect2 Rectangle;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{ 
		StartMaker = GetNodeOrNull<Marker2D>("StartMarker");
		KnifeStartPosition = StartMaker.Position;
		CollShape = GetNodeOrNull<CollisionShape2D>("SkinArea/CollisionShape2D");
		GD.Print((StartMaker == null) ? "Skinnable: StartMarker is null" : "");
		GD.Print((CollShape == null) ? "Skinnable: CollShape is Null" : "");
		
	}

	public Vector2 GetShapeLocation() {
		Rect2 rectObj = PullRectangle();
		return rectObj.Position;
	}

	/*
	public Godot.Vector2 GetSize() {
		Rect2 rectObj = PullRectangle();
		GD.Print($"Skinnable Rect Size: {rectObj.Size}");
		return rectObj.Size;
	}
	*/

	public Rect2 PullRectangle() {
		RectangleShape2D Rect = CollShape.Shape as RectangleShape2D;

		
		return Rect.GetRect();
	}
}
