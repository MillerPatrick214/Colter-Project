using Godot;
using System;

public partial class SelectableComponentTrigger : GunComponent
{
    public override Quaternion quat_counter_clock {get;set;} = new(-0.891f, 0.003f, 0.002f, 0.455f);
    public override Quaternion quat_clock {get; set;} = new(-0.991f, 0.003f, 0.0f, 0.137f);
    public override float manip_speed { get; set;} = 5.0f;


}
