using Godot;
using System;

public abstract partial class ReloadTool :TextureRect
{
    public static bool UsingTool {get; set;} =  false;
    public abstract string AreaRelativePath {get; set;} 
    public abstract Vector2 DefaultPosition {get; set;}

    public Area2D Area;
    public CollisionShape2D CollisionShape;

    public RayCast2D RayCast;
    public bool MouseOn = false;
    public bool IsSelected = false;
    public bool ReadyToUse = false;

    public abstract void Use(double delta);

    public abstract void UseExit(double delta);
    public override void _Ready()
    {
        RayCast = GetNodeOrNull<RayCast2D>("Ray");
        Area = GetNodeOrNull<Area2D>(AreaRelativePath);
        CollisionShape = Area.GetNodeOrNull<CollisionShape2D>("CollisionShape2D");


        if (Area == null || CollisionShape == null)
        {
            GD.PrintErr($"ERROR IN RELOAD TOOL {GetPath()}: One of the required Nodes returned null");
        }

        if (RayCast == null)
        {
            GD.PrintErr($"ERROR IN RELOAD TOOL {GetPath()}: Check if this node needs a raycast. Raycast returned null. If raycast not needed, ignore");
        }

        RayCast.AddException(Area);

        RayCast.Position = CollisionShape.Shape.GetRect().Size;
        
        Area.MouseEntered += () => MouseOn = true;
        Area.MouseExited += () => MouseOn = false;
    }

    public override void _Process(double delta)
    {
        Vector2 mousePos = GetGlobalMousePosition();

        if (MouseOn && !UsingTool && Input.IsActionJustPressed("UseItem")) 
        {
            IsSelected = true;
            UsingTool = true;
        }

        if (IsSelected)
        {
            Position = mousePos;

            if (Input.IsActionJustPressed("Aim"))
            {
                UseExit(delta);
                IsSelected = false;
                UsingTool = false;
                ReadyToUse = false;
            };
            if (Input.IsActionPressed("UseItem") && ReadyToUse) Use(delta);
            if (Input.IsActionJustReleased("UseItem")) {
                if (ReadyToUse) UseExit(delta);
                else ReadyToUse = true;
            }
        }
        
        else
        {
            Position = Position.Lerp(DefaultPosition, 3 * (float)delta);
		    if (Position.DistanceTo(DefaultPosition) < 0.5f)
		    {
		    	Position = DefaultPosition;
		    }
        }
    }
    
}
