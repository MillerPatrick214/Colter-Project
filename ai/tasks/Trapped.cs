using Godot;
using System;


[Tool]
public partial class Trapped : BTAction
{
    bool IsTrapped;
    public override string _GenerateName()
    {
        return "Trapped";
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
        IsTrapped = (bool)Blackboard.GetVar("IsTrapped");
        GD.PrintErr(IsTrapped);
        if (IsTrapped) return Status.Running;
        else return Status.Failure;
    }

    public override string[] _GetConfigurationWarnings()
    {
        return Array.Empty<string>();
    }

}
