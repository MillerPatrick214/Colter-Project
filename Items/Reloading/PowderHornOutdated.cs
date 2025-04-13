/*
using Godot;
using System;

public partial class PowderHorn : RigidBody3D
{

    [Export] Camera3D camera;
    bool is_mouse_over = false;
    bool is_held = false;

    RayCast3D raycast;

    GpuParticles3D powder_emitter;

    [Export]
    float pour_speed = 2.3f; //grains_per_second

    float pan_offset_x = 0.029f;

    public override void _Ready()
    {
        powder_emitter = GetNodeOrNull<GpuParticles3D>("Cone/GPUParticles3D");
        raycast = GetNodeOrNull<RayCast3D>("Cone/RayCast3D");
        MouseEntered += () => is_mouse_over = true;
        MouseExited += () => is_mouse_over = false;
        powder_emitter.Emitting = false;
    }

    public override void _Process(double delta)
    {
        Vector2 mouse_pos_vect2 = GetViewport().GetMousePosition();
        Vector3 projected_position = camera.ProjectPosition(mouse_pos_vect2, camera.Position.X - ReloadingComponent.powder_comp_x_offset);
        //raycast.TargetPosition = raycast.ToGlobal(new Vector3(0, -1.0f, 0.0f));

        if(is_mouse_over && Input.IsActionPressed("UseItem"))
        {
          
            Freeze = false;
            GlobalPosition = projected_position;
            is_held = true;
        }
        

        if (Input.IsActionJustReleased("UseItem"))
        {
            is_held = false;      
            powder_emitter.Emitting = false;      
        }

        if (is_held)
        {
            Input.MouseMode = Input.MouseModeEnum.Hidden;
            GlobalPosition = GlobalPosition.Lerp(projected_position, 5 * (float)delta);

            if (Input.IsActionPressed("Aim"))
            {
                powder_emitter.Emitting = true;
                
                GodotObject obj = raycast.GetCollider();
                if (obj is PowderMeasure measure)
                {
                    measure.Fill(pour_speed, delta);
                }

                if (obj is PanArea pan)
                {
                    
                }

            }

            if (Input.IsActionJustReleased("Aim"))
            {
                powder_emitter.Emitting = false;
            }
        }

        else
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }

        
        base._Process(delta);
    } 
}
*/
