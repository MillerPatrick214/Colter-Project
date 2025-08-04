using Godot;
using System;


[Tool]
public partial class ReloadingComponent : Control
{
    public static ReloadingComponent Instance;
    public static bool IsItemSelected = false;


    public Marker3D cam_target_marker;
    StaticBody3D curr_body_mouse_on;
    SelectableComponent ComponentMouseOn = null;
    SelectableComponent CurrentComponentSelected = null;

    [ExportGroup("Particle Collision Boxes")]
    [Export] public Marker3D OverviewMarker;
    [Export] public Marker3D MechanismMarker;
    [Export] public Marker3D BarrelEndMarker;
    [Export] public Camera3D ReloadCamera;

    BarrelHole BarrelHole;
    RodSelectableBody Rod;

    // Reloading Item data
    public static float powder_comp_x_offset = .2f;
    TextureRect Horn;
    bool IsMouseOnHorn = false;
    TextureRect PowderMeasure;
    bool IsMouseOnMeasure = false;

    //Markers for Camera Transforms ----------------
    public Transform3D OverviewTransform {get; set;}
    public Transform3D MechanismTransform {get; set;}
    public Transform3D BarrelEndTransform {get; set;}
    //-----------------------------------------------


    //--Areas for Mouse Over camera control/Hint when close -----------------------------

    public Area2D ToOverviewArea;
    public Area2D ToBarrelArea;
    public Area2D ToPanArea;
    //public HintSlerpCircle CurrentHintCircle = null;
    //bool TransformInterpolating = false; // needed so hints don't lerp position while we are manipulating transforms when transfering between markers
    //bool HintActivated = false;

    //Godot.Collections.Dictionary<HintSlerpCircle, Vector3> HintPosPairing;
    //-------------------------------------------------------------------------------------



    //Necessary Conditions
    //-----------------------------------------------------------------------------------------------
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
    //-----------------------------------------------------------------------------------------------

    [ExportGroup("Particle Collision Boxes")]
    [Export] GpuParticlesCollisionBox3D PanParticleCollision;
    [Export] GpuParticlesCollisionBox3D BarrelEndParticleCollision;

    [ExportGroup("Gun Powder Emitters")]
    [Export] StaticBody3D BarrelEmitterBody;

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

        
        if (Instance != null) return;
        Instance ??= this;
        if (OverviewMarker == null || MechanismMarker == null || BarrelEndMarker == null)
        {
            GD.PrintErr("Markers are not assigned!");
            return; // Exit early if any marker is missing
        }

        BarrelHole = GetNodeOrNull<BarrelHole>("SubViewportContainer/SubViewport/BarrelHole");
        Rod = GetNodeOrNull<RodSelectableBody>("SubViewportContainer/SubViewport/RodSelectableBody");
        Horn = GetNodeOrNull<TextureRect>("SubViewportContainer/SubViewport/Horn");
        PowderMeasure = GetNodeOrNull<TextureRect>("SubViewportContainer/SubViewport/PowderMeasure");


        //-------------------Mouse Controls for camera ----------------------------------------
        ToOverviewArea = GetNodeOrNull<Area2D>("SubViewportContainer/SubViewport/ToOverview");
        
        ToBarrelArea = GetNodeOrNull<Area2D>("SubViewportContainer/SubViewport/ToBarrel");
        ToPanArea = GetNodeOrNull<Area2D>("SubViewportContainer/SubViewport/ToPan");

        if (ToBarrelArea == null || ToPanArea == null)
        {
            GD.PrintErr("Error: To ToBarrelArea || ToPanArea is null!"); 
        }

        ToOverviewArea.MouseEntered += CamToOverview;
        ToBarrelArea.MouseEntered +=  CamToBarrelEnd;
        ToPanArea.MouseEntered += CamToMechanism;
        

        //ToBarrelArea.Hint += (circ, tf) => ManageHint(circ, tf);

        //------------------------------------------------------------------------------------

        //Copy over transforms from markers-------------------------------------------------
        OverviewTransform = OverviewMarker.Transform;
        MechanismTransform = MechanismMarker.Transform;
        BarrelEndTransform = BarrelEndMarker.Transform;
        //----------------------------------------------------------------------------------
        
        ReloadCamera.Transform = OverviewTransform;
        //PanEmitterBody.Position = new Vector3(PanParticleCollision.Position.X, PanEmitterBody.Position.Y, PanEmitterBody.Position.Z);
        //BarrelEmitterBody.Position = new Vector3(BarrelEndParticleCollision.Position.X,BarrelEmitterBody.Position.Y, BarrelEmitterBody.Position.Z);
        Rod.BarrelLoaded += RodDownBarrel;
        cam_target_marker = OverviewMarker;
       /*
        HintPosPairing = new()
        {
            { ToBarrelArea, BarrelEndMarker.GlobalPosition },
            { ToPanArea, MechanismMarker.GlobalPosition }
        };
        */

        CamToOverview();
    }

    /*
    public void ManageHint(HintSlerpCircle circ, bool tf)
    {
        CurrentHintCircle = circ;
        HintActivated = tf;

        GD.PrintErr($"HintActivated is {HintActivated}. tf is {tf}");
    }
    */
    
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
        GD.PrintErr("Bullet is in Chamber!");
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
        //HintActivated = false;
        cam_target_marker = OverviewMarker;
        ToOverviewArea.Hide();
        ToBarrelArea.Show();
        ToPanArea.Show();

    }

    public void CamToMechanism()
    {
        //HintActivated = false;
        cam_target_marker = MechanismMarker;
        ToOverviewArea.Show();
        ToBarrelArea.Hide();
        ToPanArea.Hide();
    }

    public void CamToBarrelEnd()
    {
        //HintActivated = false;
        cam_target_marker = BarrelEndMarker;
        ToOverviewArea.Show();
        ToBarrelArea.Hide();
        ToPanArea.Hide();
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
        /*
        if (HintActivated)
        {
            if (CurrentHintCircle == null) GD.PrintErr("Error in Reloading Component. CurrentHintCircle is null but HintActivated is true!");
        
            ReloadCamera.GlobalPosition = CurrentHintCircle.GetHintLerp(ReloadCamera.GlobalPosition, HintPosPairing[CurrentHintCircle], 5.0f * (float)delta);
            GD.PrintErr($"new CurrCamPosition: {ReloadCamera.GlobalPosition}");
        }
        */

        if (ReloadCamera == null) {GD.PrintErr($"ReloadCamera null {GetPath()}");}

        
        if (!ReloadCamera.Transform.IsEqualApprox(cam_target_marker.Transform)) //&& !HintActivated)
        {
            //TransformInterpolating = true;
            ReloadCamera.Transform = ReloadCamera.Transform.InterpolateWith(cam_target_marker.Transform, 5.0f * (float)delta);
        }
       /*
        else
        {
            TransformInterpolating = false;
        }
        */
    }
    
    //Quick Notes
    //
    // The pan offset will be different than the barrel end. There should probably be two particle emitters than line up with each offset (and have different gravity)
}
