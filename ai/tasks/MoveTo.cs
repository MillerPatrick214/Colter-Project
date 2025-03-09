using Godot;
using System;

[Tool]
public partial class MoveTo : BTAction
{
    Vector3 SafeVelocity;
    NPCBase agent;
    HerdComponent comp;
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

        if (Agent is Animal animal)
        {
            comp = animal.HerdComponent;
        }
    }

    public override void _Enter()
    {
    }

    public override void _Exit()
    {
        agent.NavAgent.TargetPosition = Vector3.Zero;
        agent.NavAgent.Velocity = Vector3.Zero;
        agent.Velocity = Vector3.Zero;

        if (!agent.HasNode("HerdComponent")) return;

        HerdComponent comp = agent.GetNode<HerdComponent>("HerdComponent");
       // comp.UpdatePositionWithinHerd();
    }

    public override Status _Tick(double delta)
    {
		Godot.Vector3 destination = agent.NavAgent.GetNextPathPosition();
		Godot.Vector3 LocalDestination = destination - agent.GlobalPosition;

        
		
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

