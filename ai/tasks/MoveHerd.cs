using Godot;
using System;

[Tool]
public partial class MoveHerd : BTAction
{
    

    Animal agent;
    HerdComponent comp;
    NavigationAgent3D NavAgent;

    Vector3 SafeVel;
    
    public override string _GenerateName()
    {
        return "MoveHerd";
    }

    public override void _Setup()
    {
		if (Agent is Animal agent)
        {
            this.agent = agent;

            agent.NavAgent.VelocityComputed += (vel) => SafeVel = vel;
        } 
        
    }

    public override void _Enter()
    {  
        if (comp == null) comp = agent.HerdComponent;
        if (NavAgent == null )NavAgent = agent.NavAgent;
        
        if (comp == null) GD.PrintErr($"CRAP! Comp is null in MoveHerd:{GetParent().GetPath()}!!!");

        GD.PrintErr("Entering Move Herd");
    }

    public override void _Exit()
    {
        agent.NavAgent.TargetPosition = Vector3.Zero;
        agent.NavAgent.Velocity = Vector3.Zero;
        agent.Velocity = Vector3.Zero;

        if (!agent.HasNode("HerdComponent")) return;    
    }

    public override Status _Tick(double delta)
    { 
        float min_speed = 5.0f;
        float max_speed = 13.0f; 


        Vector3 vel_vect = comp.GetBoidVelocity(min_speed, max_speed);

		vel_vect.Rotated(Vector3.Up, Mathf.DegToRad(comp.turn_bias_degrees));

        Vector3 new_target = agent.GlobalPosition + vel_vect;
		agent.NavAgent.TargetPosition = new_target;

        while (!NavAgent.IsTargetReachable())
        {
            Vector3 rotated_vect = new_target.Rotated(Vector3.Up, .25f * Mathf.Pi);
            NavAgent.TargetPosition = rotated_vect;
        }

        vel_vect = NavAgent.TargetPosition - agent.GlobalPosition;

        Godot.Vector3 destination = agent.NavAgent.GetNextPathPosition();
        Godot.Vector3 LocalDestination = destination - agent.GlobalPosition;
        Godot.Vector3 direction = LocalDestination.Normalized();
    	
		agent.Rotate(direction);
		NavAgent.Velocity = vel_vect;
        agent.Velocity = SafeVel; 

        return Status.Running;
    }

    public override string[] _GetConfigurationWarnings()
    {
        return Array.Empty<string>();
    }
}
