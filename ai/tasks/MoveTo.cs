using Godot;
using System;

[Tool]
public partial class MoveTo : BTAction
{
    Vector3 SafeVelocity;
    Vector3 Target; 
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
            agent.NavAgent.VelocityComputed += (vel) => SafeVelocity = vel;
        }
    }

    public override void _Enter()
    { 
        Target = (Vector3)Blackboard.GetVar("NavTarget");
        agent.NavAgent.TargetPosition = Target;
        
    }

    public override void _Exit()
    {
        agent.NavAgent.TargetPosition = Vector3.Zero;
        agent.NavAgent.Velocity = Vector3.Zero;
        agent.Velocity = Vector3.Zero;
    }

    public override Status _Tick(double delta)
    {
		Godot.Vector3 destination = agent.NavAgent.GetNextPathPosition();
		Godot.Vector3 LocalDestination = agent.NavAgent.GetNextPathPosition() - agent.GlobalPosition;
		Godot.Vector3 direction = LocalDestination.Normalized();

		direction.Y = 0;
		//GD.PrintErr(direction);

		if (!(direction == Vector3.Zero)) agent.Rotate(direction);
		agent.NavAgent.Velocity = direction * agent.GetWalkSpeed();
		agent.Velocity = SafeVelocity;


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

