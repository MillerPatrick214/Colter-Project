using Godot;
using System;

public partial class SelectableComponentHammer : GunComponent
{
    [Signal] public delegate void ConiditonMetEventHandler(string conditions_met);
    public override Quaternion quat_counter_clock {get; set;} = new(0.75f, 0.0f, 0.0f, 0.998f);
    
    public override Quaternion quat_clock {get; set;} = new(-0.061f, 0.0f, 0.0f, 0.998f);

    Quaternion hammer_down_quat_limit;

    bool isHalfCocked = false;
    bool isFullCocked = false;

    float hammer_spring_base_speed = 0;
    float hammer_speed_increaser = 0; 
    

    public override float manip_speed {get; set;} = 5.0f;
    
    Quaternion HalfCockQuat = new(.17f, 0.0f, 0.0f, 0.998f);

    Quaternion FullCockQuat = new(.73f, 0.0f, 0.0f, 0.998f);


    public override void _Ready()
    {
        base._Ready();
        hammer_down_quat_limit = quat_clock;
        hammer_down_quat_limit.Normalized();
        HalfCockQuat = HalfCockQuat.Normalized();
        FullCockQuat = FullCockQuat.Normalized();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (!isFullCocked)
        {
            FullCockCheck(); 
        };
        if (!isHalfCocked)
        {
            HalfCockCheck();
        }

        if(!is_selected && !curr_quat.IsEqualApprox(quat_clock))
        {  
            GD.PrintErr($"CurrQuat{curr_quat}");
            GD.PrintErr($"quat_clock{quat_clock}");

            hammer_speed_increaser += 5f;
            Quaternion slerped_quat = curr_quat.Slerp(quat_clock, hammer_speed_increaser * (float)delta);
            GD.PrintErr($"slerped_quat{slerped_quat}");

            if (slerped_quat.X <= quat_clock.X)
            {
                GD.PrintErr("Snapping Hammer to quat_clock");
                Skeleton.SetBonePoseRotation(BoneIdx, quat_clock);
                curr_quat = Skeleton.GetBonePoseRotation(BoneIdx).Normalized();
                hammer_speed_increaser = 0;
            }
            Skeleton.SetBonePoseRotation(BoneIdx, slerped_quat);
        }
    }

    public void ResetLimitsToDefault()
    {
        isHalfCocked = false;
        isFullCocked = false;
        quat_clock = hammer_down_quat_limit;
        quat_clock = quat_clock.Normalized();

    }


    public void HalfCockCheck() 
    {
        if (curr_quat.X > HalfCockQuat.X)
        {
            quat_clock = HalfCockQuat;
            quat_clock = quat_clock.Normalized();
            isHalfCocked = true;
        }
    }

    public void FullCockCheck()
    {
        if (curr_quat.X > FullCockQuat.X)
        {
            quat_clock = FullCockQuat;
            quat_clock = quat_clock.Normalized();
            isFullCocked = true;
        }
    }        

}
