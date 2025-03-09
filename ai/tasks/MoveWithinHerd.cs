using Godot;
using System;

public partial class MoveWithinHerd : BTAction
{

    NPCBase agent;
    HerdComponent comp;    public override string _GenerateName()
    {
        return "MoveWithinHerd";
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

        return Status.Success;
    }

    public override string[] _GetConfigurationWarnings()
    {
        return Array.Empty<string>();
    }
}
