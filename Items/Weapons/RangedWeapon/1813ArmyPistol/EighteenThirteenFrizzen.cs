using Godot;
using System;

public partial class EighteenThirteenFrizzen : GunComponent
{
    public override Quaternion quat_counter_clock {get; set;} = new(0.75f, 0.0f, 0.0f, .898f);
    public override Quaternion quat_clock {get; set;} = new(0.126f, 0.0f, 0.0f, .898f);
    public override float manip_speed {get; set;} = 5.0f;
}
