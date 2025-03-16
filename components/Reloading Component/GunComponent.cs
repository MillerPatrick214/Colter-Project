using Godot;
using System;

public abstract partial class GunComponent : SelectableComponent
{
    
    Vector2 mouse_start_pos;
    public bool is_selected = false;
    bool is_mouse_over = false;
    public Skeleton3D Skeleton {get; set;}
    public int BoneIdx {get; set;}
    public Quaternion curr_quat;
    public abstract Quaternion quat_counter_clock {get; set;}
    public abstract Quaternion quat_clock {get; set;}

    float mouse_rot_x = 0;

    public abstract float manip_speed {get; set;}

    public override void _Ready()
    {
        Node parent = GetParent(); 

        if (parent is not BoneAttachment3D) 
        {
            GD.PrintErr($"Error: {GetPath()} in PullSkeleton() parent was not a BoneAttachement3D!");
            return;
        }
        
        BoneAttachment3D bone_attach = (BoneAttachment3D)parent;
        Skeleton = bone_attach.GetSkeleton();
        BoneIdx = bone_attach.BoneIdx;
        curr_quat = Skeleton.GetBonePoseRotation(BoneIdx).Normalized();
        GD.PrintErr($"Starting Quat {curr_quat}");

        quat_counter_clock = quat_counter_clock.Normalized();
        quat_clock = quat_clock.Normalized();

        MouseEntered += () => is_mouse_over = true;
        MouseExited += () => is_mouse_over = false;
    }

    public override void _Process(double delta)
    {
        
        if (is_mouse_over) GD.PrintErr("IsMouseOver true!!!!");

        if (is_mouse_over && Input.IsActionJustPressed("UseItem")) 
        {
            is_selected = true;
            GD.PrintErr("Is selected");
            mouse_start_pos = GetViewport().GetMousePosition();
        }

        if (Input.IsActionJustReleased("UseItem"))
        {
            mouse_start_pos = Vector2.Zero;
            is_selected = false;
        }

        if (is_selected)
        {
            GD.PrintErr($"=--------------------------------------------------------------------------------");
            Vector2 curr_relative_mouse_pos = GetViewport().GetMousePosition() - mouse_start_pos;
            
            float magnitude = curr_relative_mouse_pos.Length();
            GD.PrintErr($"magnitude: { magnitude}");

            float cross_prod_sign = curr_relative_mouse_pos.Cross(Vector2.Up);
            

            int sign;

            sign = (MathF.Sign(cross_prod_sign) <= 0) ? 1 : -1;

            float slerp_speed = Mathf.Clamp(magnitude, 0, manip_speed);
            
            GD.PrintErr($"slerp_speed: {slerp_speed}");
            GD.PrintErr($"sign: {sign}");

            Quaternion slerped_quat = Quaternion.Identity;

            if (sign >= 0)
            {   
                slerped_quat = curr_quat.Slerp(quat_clock, slerp_speed * (float)delta);
            }

            if (sign < 0)
            {
                slerped_quat = curr_quat.Slerp(quat_counter_clock, slerp_speed * (float)delta);
            }
            
            GD.PrintErr($"slerp_speed: {slerp_speed}");

            GD.PrintErr($"slerped_quat.X {slerped_quat.X}");
            GD.PrintErr($"quat_clock.X {quat_clock.X}");
            GD.PrintErr($"quat_count_clock.Y {quat_counter_clock.X}");


            if (slerped_quat.X < quat_clock.X || slerped_quat.X > quat_counter_clock.X)
            {
                GD.PrintErr("SLERPED QUATERNION TOO BIG/SMALL --- RETURNING WITHOUT CHANGING ANGLE");
                return;
            }

            Skeleton.SetBonePoseRotation(BoneIdx, slerped_quat);

            curr_quat = Skeleton.GetBonePoseRotation(BoneIdx).Normalized();            
        }
    }
    
    public void Highlight()
    {

    }
}

