using Godot;
using System;
using System.Drawing;
using System.Dynamic;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;



public partial class Skinnable : Sprite2D
{
	[Signal]
	public delegate void MouseOnSkinEventHandler(bool isTrue);

	

	public Godot.Vector2 KnifeStartPosition;
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

	public Godot.Vector2 GetShapeLocation() {
		Rect2 rectObj = PullRectangle();
		GD.Print($"Skinnable Rect Position: {rectObj.Position}");
		return rectObj.Position;
	}

	public Godot.Vector2 GetSize() {
		Rect2 rectObj = PullRectangle();
		GD.Print($"Skinnable Rect Size: {rectObj.Size}");
		return rectObj.Size;
	}

	public Rect2 PullRectangle() {
		RectangleShape2D Rect = CollShape.Shape as RectangleShape2D;

		
		return Rect.GetRect();
	}
}
