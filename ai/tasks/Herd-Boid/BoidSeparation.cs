using Godot;
using System;


//https://www.red3d.com/cwr/boids/

[Tool]
public partial class BoidSeparation: BTAction
{
    public override string _GenerateName()
    {
        return "BoidSeparation";
    }

    public override void _Setup()
    {
    }

    public override void _Enter()
    {

    }

    public override void _Exit()
    {
        
    }

    public override Status _Tick(double delta)
    {
        return Status.Running;
    }

    public override string[] _GetConfigurationWarnings()
    {
        return Array.Empty<string>();
    }
}