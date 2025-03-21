using Godot;
using System;

public partial class PowderMeasure : RigidBody3D
{
    [Export] Camera3D camera;
    
    [Export]
    public float max_fill = 25.0f;
    public float curr_fill = 0.0f;
    public bool full = false;
    public bool is_mouse_over = false;
    bool is_held = false;

    MeshInstance3D PowderFullIndic;     //Fix me in future with proper texture;

    
    public override void _Ready()
    {
        Position = new(ReloadingComponent.powder_comp_x_offset, Position.Y, Position.Z);
        PowderFullIndic = GetNodeOrNull<MeshInstance3D>("PowderFullIndic");
        PowderFullIndic.Hide();
        MouseEntered += () => is_mouse_over = true;
        MouseExited += () => is_mouse_over = false;
    }

    public override void _Process(double delta)
    {
        Vector2 mouse_pos_vect2 = GetViewport().GetMousePosition();

        Vector3 projected_position;

        if (ReloadingComponent.Instance.cam_target_marker == ReloadingComponent.Instance.BarrelEndMarker)     //if rotation lines up with the end-of-barrel marker camera
        {
            projected_position = camera.ProjectPosition(mouse_pos_vect2, camera.Position.X);    // Line up with x axis/ barrel end
            
        }
        else
        {
            projected_position = camera.ProjectPosition(mouse_pos_vect2, camera.Position.X - ReloadingComponent.powder_comp_x_offset);
        }

        if(is_mouse_over && Input.IsActionPressed("UseItem"))
        {
          
            Freeze = false;
            GlobalPosition = projected_position;
            is_held = true;
        }

        if (Input.IsActionJustReleased("UseItem"))
        {
            is_held = false;            
        }

        if (is_held)
        {
            Input.MouseMode = Input.MouseModeEnum.Hidden;
            GlobalPosition = GlobalPosition.Lerp(projected_position, 5 * (float)delta);
        }
        else
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }

        
        base._Process(delta);
    }

    public void Fill(float pour_speed, double delta) //GPS being grains per second lmao
    {
        curr_fill += pour_speed * .10f * (float)delta;
        if (curr_fill >= max_fill)
        {
            full = true;
            PowderFullIndic.Show();
        }
    }

    public void Pour()
    {

    }
}
