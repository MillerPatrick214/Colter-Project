using Godot;
using System;



public partial class Humanoid : NPCBase
{
    
    [Export]
    public Marker3D itemMarker;
    RangedWeapon Equipped; //Should probably just be a Weapon?
    
    //TEMP-----------------------------------------------------------------------------
    [Export]
    float DeviationConeAngleDegrees = 1.0f;
    string AmmoPath; 
    float ProjectileVelocity;
    PackedScene AmmoScene;
    bool CanFire;
    public Marker3D TEMPFIRE;
    //----------------------------------------------------------------------------------
    public override void _Ready()
    {
        base._Ready();
        
        //TEMP ------------------------------------------------------------------------------
        if (itemMarker == null) {GD.PrintErr("ERROR Humanoid: No ItemMarker set in Editor");}
        Equipped = itemMarker.GetChildOrNull<RangedWeapon>(0);
        if (Equipped != null)
        {
            AmmoPath = Equipped.AmmoPath;
            AmmoScene = GD.Load<PackedScene>(AmmoPath);
            TEMPFIRE = GetNodeOrNull<Marker3D>("TEMPFIRE");
            if (TEMPFIRE == null)
            {
                GD.PrintErr("Temp Fire RETURNED NULL GAH DAM<MIT");
            }
            ProjectileVelocity = Equipped.ProjectileVelocity;
        }
        //-----------------------------------------------------------------------------------
    }

    public void Equip(int index)
    {
        //TODO 
    } 

    public void TEMPFire() {
		Equipped.AniTree.Set("parameters/OneShot/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);	
		RigidBody3D ProjectileInstance = AmmoScene.Instantiate<RigidBody3D>();
		GetTree().Root.AddChild(ProjectileInstance);
		ProjectileInstance.GlobalPosition = TEMPFIRE.GlobalPosition;
        
        RandomNumberGenerator rand = new();

        Vector3 ZRand = -TEMPFIRE.GlobalTransform.Basis.Z; 

        float ConeAngleRads = Mathf.DegToRad(DeviationConeAngleDegrees);

        float randomYaw = rand.RandfRange(-ConeAngleRads/2, ConeAngleRads/2);
        float randomPitch = rand.RandfRange(-ConeAngleRads/2, ConeAngleRads/2);

        ZRand = ZRand.Rotated(Vector3.Up, randomYaw);
        ZRand = ZRand.Rotated(ZRand.Cross(Vector3.Up).Normalized(), randomPitch);

        ZRand = ZRand.Normalized();

		ProjectileInstance.ApplyCentralImpulse(ZRand * ProjectileVelocity);
	}
}
