using Godot;
using System;

public partial class PowderMeasure : ReloadTool
{
    Marker2D marker; 
    public RichTextLabel DebugText;
    public override Vector2 DefaultPosition {get; set;} = new();
    public override string AreaRelativePath {get; set;}  = "MeasureArea";
    public float current_fill;
    public float max_fill {get; set;} = 20;

    public override void _Ready()
    {
        base._Ready();
        DebugText = GetNodeOrNull<RichTextLabel>("RichTextLabel");
        if (DebugText == null)
        {
            GD.PrintErr("PowderMeasure: Error -- DebugText node reference is null");
        }
        DebugText.Hide();
    }

    public override void Use(double delta)
    {

    }

    public override void UseExit(double delta)
    {
        
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        
        if (marker == null)
        {
            GD.PrintErr("DefaultCupPos marker not set in PowderMeasure export. Attempting to locate on my own...");
            
            marker = GetParent().GetNodeOrNull<Marker2D>("DefaultCupPos");

            if (marker == null) GD.PrintErr("Error in PowderMeasure: Unable to obtain DefaultCupPos marker");

            else 
            {
                GlobalPosition = marker.GlobalPosition;
                DefaultPosition = marker.GlobalPosition;
            }
        }
    }

    public void Fill(float pour_rate)
    {
        if (current_fill >= max_fill)
        {
            DebugText.Show();
            return;
        }
        current_fill += pour_rate;
        GD.PrintErr($"Current Fill: {current_fill}");
    }

    public void PourIntoBarrel()
    {
        DebugText.Hide();
        current_fill = 0;

    }
}
