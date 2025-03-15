using Godot;
using System;

[Tool]
public partial class ReloadingComponent : Node3D
{
    Marker3D TargetMarker;

    [Export] Marker3D OverviewMarker;
    [Export] Marker3D MechanismMarker;
    [Export] Marker3D BarrelEndMarker;
    [Export] Camera3D ReloadCamera;

    [Export] GpuParticlesCollisionBox3D PanParticleCollision;
    [Export] GpuParticlesCollisionBox3D BarrelEndParticleCollision;

    [Export] StaticBody3D BarrelParticleBody;

    [Export] StaticBody3D PanParticleBody;

    [ExportToolButton("Set Camera to Finish/End Marker")] public Callable OverviewButton => Callable.From(CamToOverview);
    [ExportToolButton("Set Camera to Mechanism Marker")]  public Callable MechanismButton => Callable.From(CamToMechanism);
    [ExportToolButton("Set Camera to Barrel End Marker")] public Callable BarrelEndButtion => Callable.From(CamToBarrelEnd);

    public override void _Ready()
    {
        ReloadCamera.Transform = OverviewMarker.Transform;
        BarrelParticleBody.Position = new Vector3(PanParticleCollision.Position.X,BarrelParticleBody.Position.Y, BarrelParticleBody.Position.Z);
    }


    public void CamToOverview()
    {
        TargetMarker = OverviewMarker;
    }

    public void CamToMechanism()
    {
        TargetMarker = MechanismMarker;
    }

    public void CamToBarrelEnd()
    {
        TargetMarker = BarrelEndMarker;
    }

    public override void _Process(double delta)
    {
        if (!ReloadCamera.Transform.IsEqualApprox(TargetMarker.Transform))
        {
            ReloadCamera.Transform = ReloadCamera.Transform.InterpolateWith(TargetMarker.Transform, 5 * (float)delta);
        }
    }
    
    //Quick Notes
    //
    // The pan offset will be different than the barrel end. There should probably be two particle emitters than line up with each offset (and have different gravity)
}
