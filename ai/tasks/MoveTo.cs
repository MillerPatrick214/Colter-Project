using Godot;
using System;

[Tool]
public partial class MoveTo : BTAction
{
    NPCBase agent;
    public override string _GenerateName()
    {
        return "MoveTo";
    }

    public override void _Setup()
    {
        if (Agent is NPCBase agent)
        {
            this.agent = agent;
        }
    }

    public override void _Enter()
    {
        
    }

    public override void _Exit()
    {
        agent.NavAgent.TargetPosition = Vector3.Zero;
        agent.Velocity = Vector3.Zero;
    }

    public override Status _Tick(double delta)
    {
		Godot.Vector3 destination = agent.NavAgent.GetNextPathPosition();
		Godot.Vector3 LocalDestination = destination - agent.GlobalPosition;
		
		Godot.Vector3 direction = LocalDestination.Normalized();

		direction.Y = 0;
		//GD.PrintErr(direction);

		agent.Rotate(direction);
	
		agent.Velocity = direction * agent.GetWalkSpeed();


        if (agent.NavAgent.IsNavigationFinished()){
            return Status.Success;
        }
        
        return Status.Running;
    }

    public override string[] _GetConfigurationWarnings()
    {
        return Array.Empty<string>();
    }
}

