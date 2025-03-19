using Godot;
using System;
using System.Threading.Tasks;
public partial class BallSelectableBody : RigidBody3D
{
    [Export] Camera3D camera;
    bool is_mouse_over = false;
    bool snap_override = false;
    bool is_held = false;
    bool push_down = false; //currently accessed from reloading to switch on push down barrel lmao. Not good design

    SceneTreeTimer timer;
    public override void _Ready()
    {
        base._Ready();
        MouseEntered += () => is_mouse_over = true;
        MouseExited += () => is_mouse_over = false;
        
    }
    public override void _Process(double delta)
    {
        Vector2 mouse_pos_vect2 = GetViewport().GetMousePosition();
        

        Vector3 projected_position  = camera.ProjectPosition(mouse_pos_vect2, camera.Position.X);
        projected_position = new Vector3 (0, projected_position.Y, projected_position.Z);

        if (push_down)
        {
            if (timer == null)
            {
                timer ??= GetTree().CreateTimer(250);
                timer.Timeout += QueueFree;
            }
            PushDownMovement(delta);
            return;
        }

        if(is_mouse_over && Input.IsActionPressed("UseItem"))
        {
          
            Freeze = false;
            GlobalPosition = projected_position;
            is_held = true;
        }

        if (Input.IsActionJustReleased("UseItem"))
        {
            snap_override = false;
            is_held = false;            
        }

        if (is_held && !snap_override)
        {
            Input.MouseMode = Input.MouseModeEnum.Hidden;
            GlobalPosition = GlobalPosition.Lerp(projected_position, 5 * (float)delta);
        }
        else
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
    }
    public void SnapToBarrelTip(Vector3 TipGlobalPos)
    {
        Freeze = true;
        is_held = false;
        snap_override = true;
    }
    
    public void PushDown()
    {
        push_down = true;
    }

    public void PushDownMovement(double delta)
    {
        Freeze = true;
        GlobalPosition = GlobalPosition.Lerp(GlobalPosition, 10 * (float)delta);
    }    
}
