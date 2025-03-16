using Godot;
using System;

public partial class ReloadSettings : Resource
{
    public Transform3D OverviewTransform {get; set;}
    public Transform3D MechanismTransform {get; set;}
    public Transform3D BarrelEndTransform {get; set;}
    public ReloadSettings(Transform3D OverviewTransform, Transform3D MechanismTransform, Transform3D BarrelEndTransform)
    {
        this.OverviewTransform = OverviewTransform;
        this.MechanismTransform = MechanismTransform;
        this.BarrelEndTransform = BarrelEndTransform; 
    }
    
    
}
