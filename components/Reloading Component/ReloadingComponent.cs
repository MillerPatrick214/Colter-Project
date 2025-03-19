using Godot;
using System;


[Tool]
public partial class ReloadingComponent : Control
{

    [Signal] public delegate void ReloadDisabledEventHandler(ReloadingComponent self);
    Marker3D cam_target_marker;
    StaticBody3D curr_body_mouse_on;

    
    
    SelectableComponent ComponentMouseOn = null;
    SelectableComponent CurrentComponentSelected = null;

    [ExportGroup("Particle Collision Boxes")]
    [Export] Marker3D OverviewMarker;
    [Export] Marker3D MechanismMarker;
    [Export] Marker3D BarrelEndMarker;
    [Export] Camera3D ReloadCamera;

    BarrelHole BarrelHole;
    RodSelectableBody Rod;

    public Transform3D OverviewTransform {get; set;}
    public Transform3D MechanismTransform {get; set;}
    public Transform3D BarrelEndTransform {get; set;}


    //Necessary Conditions
    //--------------------
    bool HammerFullCock = false;
    bool PanPrimed = false;
    bool FrizzenDown = false;
    bool BulletInChamber = false;
    bool PowderInChamber = false;

    bool HoldingBarrelItem = false; //Controls lerp to barrel;


/*

    [Flags]                                 I think we should use bitwise operators for this shee. Will hafve to link up with Jarred or learn. 
    enum conditions_met
    {
        HammerFullCock,
        PanPrimed,
        FrizzenDown,
        PowderInBarrel,
        BulletInBarrel,
        BulletFullySeated,
    }    

    */

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
    [ExportToolButton("Set Camera to Overview Marker")] public Callable OverviewButton => Callable.From(CamToOverview);
    [ExportToolButton("Set Camera to Mechanism Marker")]  public Callable MechanismButton => Callable.From(CamToMechanism);
    [ExportToolButton("Set Camera to Barrel End Marker")] public Callable BarrelEndButton => Callable.From(CamToBarrelEnd);
    [ExportToolButton("Make Children Editable")] public Callable MakeEditableButton => Callable.From(MakeEditable);

    [ExportGroup("Save & Load Settings")]
    [ExportSubgroup("Save")]
    [ExportToolButton("Save Settings Resource")] public Callable SaveTresButton => Callable.From(SaveSettings);
    [Export] public string FileName = "";
    [Export] public string Path = "";
    [Export] public EditorPaths ResourceDestination;
    [Export] bool Override_Existing = false;


    [ExportSubgroup("Load")]
    [ExportToolButton("Load Settings Resource")] public Callable LoadTresButton => Callable.From(LoadSettings);
    [Export] public ReloadSettings SettingsResource;
    

    public override void _Ready()
    {
        if (OverviewMarker == null || MechanismMarker == null || BarrelEndMarker == null)
        {
            GD.PrintErr("Markers are not assigned!");
            return; // Exit early if any marker is missing
        }

        BarrelHole = GetNodeOrNull<BarrelHole>("SubViewportContainer/SubViewport/BarrelHole");
        Rod = GetNodeOrNull<RodSelectableBody>("SubViewportContainer/SubViewport/RodSelectableBody");
        OverviewTransform = OverviewMarker.Transform;
        MechanismTransform = MechanismMarker.Transform;
        BarrelEndTransform = BarrelEndMarker.Transform;

        
        ReloadCamera.Transform = OverviewTransform;
        //PanEmitterBody.Position = new Vector3(PanParticleCollision.Position.X, PanEmitterBody.Position.Y, PanEmitterBody.Position.Z);
        //BarrelEmitterBody.Position = new Vector3(BarrelEndParticleCollision.Position.X,BarrelEmitterBody.Position.Y, BarrelEmitterBody.Position.Z);
        Rod.BarrelLoaded += RodDownBarrel;
        cam_target_marker = OverviewMarker;
    }
    public void SaveSettings()
    {
        ResourceSaver.Save(new ReloadSettings(OverviewTransform, MechanismTransform, BarrelEndTransform));
        
    }

    public void RodDownBarrel()
    {
        BallSelectableBody ball = BarrelHole.InTip;
        GD.PrintErr ($"RodDownBarrel Called: BarrelHole.InTip = {ball}");
        if (ball == null) return;
        ball.PushDown();
        BulletInChamber = true;
    }

    public void PowderIntoChamber()
    {
        if (BulletInChamber) return;

        //await trigger pull, once trigger pulled annouce ReloadDisabled and emit signal
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
        if (GetParent() == null)
        {
            GD.PrintErr($"Parent returned null. ReloadingComponent is Likely the Parent");
            return;
        }

        GetParent().SetEditableInstance(this, !children_editable);
        children_editable = !children_editable;
    }

    public override void _Process(double delta)
    {
        if (ReloadCamera == null) 
        {
            GD.PrintErr($"ReloadCamera is null at {GetPath()}");
            return; // Avoid processing if ReloadCamera is null
        }
        
        if (cam_target_marker == null) 
        {
            GD.PrintErr($"cam_target_marker is null at {GetPath()}");
            return; // Avoid processing if cam_target_marker is null
        }

        if (ReloadCamera == null) {GD.PrintErr($"ReloadCamera null {GetPath()}");}
        if (!ReloadCamera.Transform.IsEqualApprox(cam_target_marker.Transform))
        {
            ReloadCamera.Transform = ReloadCamera.Transform.InterpolateWith(cam_target_marker.Transform, 5.0f * (float)delta);
        }
        if (Input.IsActionJustPressed("ui_up"))
        {
            cam_target_marker = MechanismMarker;
            return;

        }
        if (Input.IsActionPressed("ui_left"))
        {
            cam_target_marker = OverviewMarker;
            return;

        }
        if(Input.IsActionPressed("ui_right"))
        {
            cam_target_marker = BarrelEndMarker;
            return;
            
        }
        if (Input.IsActionJustPressed("ui_down"))
        {
            cam_target_marker = OverviewMarker;
            return;
        }
    }
    
    //Quick Notes
    //
    // The pan offset will be different than the barrel end. There should probably be two particle emitters than line up with each offset (and have different gravity)
}
