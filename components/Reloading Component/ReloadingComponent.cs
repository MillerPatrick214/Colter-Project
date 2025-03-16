using Godot;
using System;


[Tool]
public partial class ReloadingComponent : Node3D
{
    Marker3D cam_target_marker;
    StaticBody3D curr_body_mouse_on;

    bool is_interacting;
    
    SelectableComponent ComponentMouseOn = null;
    SelectableComponent CurrentComponentSelected = null;

    [ExportGroup("Particle Collision Boxes")]
    [Export] Marker3D OverviewMarker;
    [Export] Marker3D MechanismMarker;
    [Export] Marker3D BarrelEndMarker;
    [Export] Camera3D ReloadCamera;

    public Transform3D OverviewTransform {get; set;}
    public Transform3D MechanismTransform {get; set;}
    public Transform3D BarrelEndTransform {get; set;}

    [Flags]
    enum conditions_met
    {
        HammerFullCock,
        PanPrimed,
        FrizzenDown,
        PowderInBarrel,
        BulletInBarrel,
        BulletFullySeated,
    }    

    [ExportGroup("Particle Collision Boxes")]
    [Export] GpuParticlesCollisionBox3D PanParticleCollision;
    [Export] GpuParticlesCollisionBox3D BarrelEndParticleCollision;

    [ExportGroup("Gun Powder Emitters")]
    [Export] StaticBody3D BarrelEmitterBody;
    [Export] StaticBody3D PanEmitterBody;

    /// <summary>
    /// Array for all Selectable Bodies in the Scene. Includes 
    /// </summary>
    [ExportGroup("Selectable Bodies")]
    [Export] Godot.Collections.Array<StaticBody3D> SelectableBodies;


    bool children_editable = false;

    [ExportGroup("Tool Buttons")]
    [ExportToolButton("Set Camera to Finish/End Marker")] public Callable OverviewButton => Callable.From(CamToOverview);
    [ExportToolButton("Set Camera to Mechanism Marker")]  public Callable MechanismButton => Callable.From(CamToMechanism);
    [ExportToolButton("Set Camera to Barrel End Marker")] public Callable BarrelEndButton => Callable.From(CamToBarrelEnd);
    [ExportToolButton("Make Children Editable")] public Callable MakeEditableButton => Callable.From(MakeEditable);

    /*
    [ExportGroup("Save & Load Settings")]
    [ExportSubgroup("Save")]
    [ExportToolButton("Save Settings Resource")] public Callable SaveTresButton => Callable.From(SaveSettings);
    [Export] public string FileName = "";
    [Export] public EditorPaths ResourceDestination;
    [Export] bool Override_Existing = false;


    [ExportSubgroup("Load")]
    [ExportToolButton("Load Settings Resource")] public Callable LoadTresButton => Callable.From(LoadSettings);
    [Export] public EditorPaths ResourceLocation;
    */

    public override void _Ready()
    {
        ReloadCamera.Transform = OverviewMarker.Transform;
        PanEmitterBody.Position = new Vector3(PanParticleCollision.Position.X, PanEmitterBody.Position.Y, PanEmitterBody.Position.Z);
        BarrelEmitterBody.Position = new Vector3(BarrelEndParticleCollision.Position.X,BarrelEmitterBody.Position.Y, BarrelEmitterBody.Position.Z);
        cam_target_marker = OverviewMarker;
    }
    public void SaveSettings()
    {
        //ResourceSaver.Save();
    }
    public void LoadSettings()
    {

    }
    public void CamToOverview()
    {
        cam_target_marker = OverviewMarker;
    }

    public void CamToMechanism()
    {
        cam_target_marker = MechanismMarker;
    }

    public void CamToBarrelEnd()
    {
        cam_target_marker = BarrelEndMarker;
    }

    public void MakeEditable()
    {
        GetParent().SetEditableInstance(this, !children_editable);
        children_editable = !children_editable;
    }

    public override void _Process(double delta)
    {
        if (!ReloadCamera.Transform.IsEqualApprox(cam_target_marker.Transform))
        {
            ReloadCamera.Transform = ReloadCamera.Transform.InterpolateWith(cam_target_marker.Transform, 5.0f * (float)delta);
        }
    }
    
    //Quick Notes
    //
    // The pan offset will be different than the barrel end. There should probably be two particle emitters than line up with each offset (and have different gravity)
}
