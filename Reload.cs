using Godot;
using System;
using System.Drawing;

[Tool]
public partial class Reload : Control
{
    [Export] public TextureRect BarrelTextRect;
    public Area2D BarrelOpening;
    public CollisionShape2D BarrelOpeningCollisionShape;

    public Vector2 curr_text_size;



    public override void _Ready()
    {
        base._Ready();
        BarrelOpening = BarrelTextRect.GetNode<Area2D>("BarrelOpening");
        BarrelOpeningCollisionShape = BarrelOpening.GetNodeOrNull<CollisionShape2D>("OpeningCollision");

        curr_text_size = BarrelTextRect.Texture.GetSize();
        RectangleShape2D rect_shape = (RectangleShape2D)BarrelOpeningCollisionShape.Shape;

        rect_shape.Size = new Vector2(curr_text_size.X, 25);
        BarrelOpeningCollisionShape.Shape = rect_shape;
        
        ResizeChildren();
        
        BarrelTextRect.Texture.Changed += ResizeChildren;
        this.Resized += ResizeChildren;
    }

    public override void _Process(double delta)
    {
    }

    public void ResizeChildren()
    {
        if (BarrelOpening == null) GD.PrintErr("Reload Scene Error! No barrel opening node found. Will not properly resize");
        Vector2 old_text_size = curr_text_size;
        Vector2 new_text_size = BarrelTextRect.Texture.GetSize();

        CollisionShape2D BarrelOpeningCollisionShape = BarrelOpening.GetNodeOrNull<CollisionShape2D>("OpeningCollision");
        
        RectangleShape2D curr_shape = (RectangleShape2D)BarrelOpeningCollisionShape.Shape;
        float y_offset = (curr_shape.Size.Y/2);
        
        float new_x = ((curr_shape.Size.X/old_text_size.X)*new_text_size.X);
        float new_y = ((curr_shape.Size.Y/old_text_size.Y)*new_text_size.Y) + y_offset;
        Vector2 new_shape_size = new(new_x, new_y);
        curr_shape.Size = new_shape_size;
        
        BarrelOpening.Position =  new Vector2 (new_text_size.X/2, y_offset);
        curr_text_size = new_text_size;
        
        
    }   
}
