using Godot;
using System;

[Tool]
public partial class Fire : BTAction
{
    Humanoid agent;
    public override string _GenerateName()
    {
        return "Fire";
    }

    public override void _Setup()
    {
        if (Agent is Humanoid agent) {this.agent = agent;}
    }

    public override void _Enter()
    {
    }

    public override void _Exit()
    {
    }

    public override Status _Tick(double delta)
    {
        agent.TEMPFire();
        return Status.Success;
    }

    public override string[] _GetConfigurationWarnings()
    {
        return Array.Empty<string>();
    }
}
