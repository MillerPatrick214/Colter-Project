using Godot;
using System;

[Tool]
public partial class Freeze : BTAction
{
    public override string _GenerateName()
    {
        return "Freeze";
    }

    public override void _Setup()
    {
    }

    public override void _Enter()
    {
        if (Agent is NPCBase agent)
        {
            agent.Velocity = Vector3.Zero;
            agent.NavAgent.TargetPosition = Vector3.Zero;
        }
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

