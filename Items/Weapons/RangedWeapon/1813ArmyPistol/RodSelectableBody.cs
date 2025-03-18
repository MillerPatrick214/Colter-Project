using Godot;
using System;

[Tool]
public partial class RodSelectableBody : StaticBody3D
{

    [ExportToolButton("Print Current Transform (in C# for new Tranform3D)")] public Callable PrintTransformButton => Callable.From(PrintTransform);
    
    public RodState curr_rod_state = RodState.STOWED;

    bool isMouseOn = false;
    
    public enum RodState 
    {
        STOWED,
        //FREE,
        IN_BARREL
    }

    Transform3D StartPosition = new Transform3D(1f, 0f, 0f, 0f, 0.9999996f, 0f, 0f, 0f, 0.9999996f, 0f, -0.003f, -0.11f); 
    Transform3D ReadyPosition = new Transform3D(1f, 0f, 0f, 0f, 0.866025f, -0.5f, 0f, 0.5f, 0.866025f, 0f, -0.105f, -0.37f);
    Transform3D InBarrelTip = new Transform3D(1f, 0f, 0f, 0f, 0.9975641f, 0.06975647f, 0f, -0.06975647f, 0.9975641f, -0.0002104938f, -0.05027461f, -0.3165802f);
    Transform3D DownBarrel= new Transform3D(1f, 0f, 0f, 0f, 0.9986295f, 0.05233596f, 0f, -0.05233596f, 0.9986295f, -0.0002104938f, -0.048186004f, -0.19389842f);

    public override void _Ready()
    {
        MouseEntered += () => isMouseOn = true;
        MouseExited += () => isMouseOn = false;
    }

    public void PrintTransform()
    {
        GD.PrintRich($"[color=green]new Transform3D({Transform}); [/color]");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        switch (curr_rod_state)
        {
            case RodState.STOWED:
            StowedProcess(delta);
            return;
            
            /*
            case RodState.FREE:
            FreeProcess(delta);
            return;
            */


            case RodState.IN_BARREL:
            InBarrelProcess(delta);
            return;
        }
    }

    public void StowedProcess(double delta)
    {
        if (isMouseOn && Input.IsActionJustPressed("UseItem"))
        {
                        GD.PrintErr("Stowed process");

            StowedToBarrel(delta);
        }

    }

    public void StowedToBarrel(double delta)
    {
        while (!Transform.IsEqualApprox(ReadyPosition))
        {
            GD.PrintErr("Reeeee");
            Transform = Transform.InterpolateWith(ReadyPosition, 10f * (float)delta);
        }
        while (!Transform.IsEqualApprox(InBarrelTip))
        {
            Transform = Transform.InterpolateWith(InBarrelTip, 10f * (float)delta);
        }

        for (int i = 0; i < 2; ++i)
        {
            while (!Transform.IsEqualApprox(DownBarrel))
            {
                Transform = Transform.InterpolateWith(DownBarrel, 40f * (float)delta);
            }

            
            while (!Transform.IsEqualApprox(InBarrelTip))
            {
                Transform = Transform.InterpolateWith(DownBarrel, 30f * (float)delta);
            }
        }

        while (!Transform.IsEqualApprox(DownBarrel))
        {
            Transform = Transform.InterpolateWith(DownBarrel, 50f * (float)delta);   
        }

        curr_rod_state = RodState.IN_BARREL;
        return;
    }

    public void InBarrelProcess(double delta)
    {
        if (isMouseOn && Input.IsActionJustPressed("UseItem"))
        {
            BarrelToStowed(delta);
        }

    }

    public void BarrelToStowed(double delta)
    {
        while (!Transform.IsEqualApprox(InBarrelTip))
        {
            Transform = Transform.InterpolateWith(DownBarrel, 30f * (float)delta);
        }

        while (!Transform.IsEqualApprox(ReadyPosition))
        {
            Transform = Transform.InterpolateWith(ReadyPosition, 10f * (float)delta);
        }

        while (!Transform.IsEqualApprox(StartPosition))
        {
            Transform = Transform.InterpolateWith(StartPosition, 10f * (float)delta);
        }


        curr_rod_state = RodState.STOWED;
        return;
    }
}

    /*
    public void FreeProcess(double delta)
    {
        Vector2 mouse_pos = GetViewport().GetMousePosition();
        Vector3 converted_pos = new Vector3(0.0f, mouse_pos.Y, mouse_pos.X);
        Position = Position.Lerp(converted_pos, 20.0f * (float)delta);
        

        if()
        if(//TouchesGunBodyHole?Idklol1)
    }
    *.

    public void FreeToBarrelTransition(double delta)
    {
        
    }

    public void InBarrelProcess(double delta)
    {

    }

    public void PrintEachElement(Vector3 vect)
    {

        for(int i = 0; i < 3; ++i)
        {
            GD.PrintRich($"[color=blue]{vect[i]}[/color],");
        }
    }


    public void FreeState(Vector2 mouse_pos, double delta)
    {
        Position = new Vector3(0.0f, mouse_pos.Y, mouse_pos.X);
    }
    */

/*
public partial class RamRodBehavior : Node
{
    public virtual void StateProcess(double delta)
    {

    }

    public virtual void Transition(double delta)
    {

    }



}

public partial class Stowed : RamRodBehavior
{
    public override void StateProcess(double delta)
    {
        
        
    }

    public virtual void Transition(double delta,)
    {
        while (!Transform.IsEqualApprox(ReadyPosition))
        {
            Transform = Transform.InterpolateWith(ReadyPosition, 10f * (float)delta);
        }

    }

}
*/


