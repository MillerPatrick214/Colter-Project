using Godot;
using System;

public partial class HintSlerpCircle : Area2D
{
    [Signal] public delegate void HintEventHandler(HintSlerpCircle self, bool activated);
    Area2D HintZone;
    public bool MouseOn;

    public float max_distance_possible;
    public override void _Ready()
    {
        base._Ready();

        HintZone = GetNodeOrNull<Area2D>("HintZone");

        HintZone.MouseEntered += () => MouseToggle(true);
        HintZone.MouseExited += () => MouseToggle(false);

        CollisionShape2D my_coll_shape;
        CollisionShape2D hint_coll_shape;

        my_coll_shape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
        hint_coll_shape = GetNodeOrNull<CollisionShape2D>("HintZone/CollisionShape2D");

        CircleShape2D my_circle = my_coll_shape.Shape as CircleShape2D;
        CircleShape2D hint_circle = hint_coll_shape.Shape as CircleShape2D;

        max_distance_possible = hint_circle.Radius;
    }

    public void MouseToggle(bool tf)
    {
        MouseOn = tf;
        EmitSignal(SignalName.Hint, this, tf);
    }

    /*
    public Vector3 GetMaxHintPos(Vector3 global_cam__pos, Vector3 global_target)
    {
        Vector3 curr_pos = global_cam__pos;
        Vector3 target_pos = global_target;
        Vector3 max_pos = (curr_pos + target_pos)/4;
        return max_pos;
    }


    public Vector3 GetLerpedPos(Vector3 global_cam__pos, Vector3 global_max_pos, double delta)                                    //This really should be a pos shift w/ a moderate transform/quaternion change with the hint I think. Not 100% sure yet. Need to see it in action
    {
        float distance = Mathf.Clamp(GetGlobalMousePosition().DistanceTo(new Vector2(global_max_pos.Z, global_max_pos.Y)), 0, max_distance_possible)/max_distance_possible;
        GD.PrintErr(distance);
        Vector3 new_pos = global_cam__pos.Lerp(global_max_pos * (1.0f - distance), 2.0f * (float)delta);        
        return new_pos; 
    }
    */

    public Vector3 GetHintLerp(Vector3 global_cam_pos, Vector3 global_target_pos, double delta)
    {
        GD.PrintErr($"GetGlobalMousePosition(): {GetGlobalMousePosition()}");
        GD.PrintErr($"this GlobalPosition {GlobalPosition}");
        GD.PrintErr($"max_distance_possible{max_distance_possible}");

        float distance_factor = 1.0f - GetGlobalMousePosition().DistanceTo(GlobalPosition)/max_distance_possible;
        GD.PrintErr($"distance_factor: {distance_factor}");
        Vector3 hint_lerp = global_cam_pos.Lerp(global_target_pos * distance_factor /5, 2.0f * (float)delta);
        hint_lerp = new(global_cam_pos.X, hint_lerp.Y, hint_lerp.Z);
        return hint_lerp;
    }
}

