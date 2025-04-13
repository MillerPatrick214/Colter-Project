using Godot;
using System;

public partial class PowderHorn : ReloadTool
{
    public override Vector2 DefaultPosition {get; set;} = new();
    public override string AreaRelativePath {get; set;}  = "HornArea";
    GpuParticles2D Particles; 
    float pour_rate = 20f;

    public override void Use(double delta)
    {
        Particles.Emitting = true;
        GodotObject Collision = RayCast.GetCollider();
        GD.PrintErr($"Collision RayCast Horn {Collision}");
        if (Collision is Area2D area)
        {
            if (area.GetParent() is not PowderMeasure measure) return;
            GD.PrintErr("Calling Fill on powder Measure");
            measure.Fill(pour_rate * (float)delta);  
        }
        //If collision is pan, zoom into pan.
    }
    public override void UseExit(double delta)
    {
        Particles.Emitting = false;
    }

    public override void _Ready()
    {
        base._Ready();
        Particles = GetNodeOrNull<GpuParticles2D>("GPUParticles2D");
        Particles.Emitting = false;
        Particles.Position = CollisionShape.Shape.GetRect().Size;
    }

}
