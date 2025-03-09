using Godot;
using System;

[Tool]
public partial class MoveHerd : BTAction
{
    

    Animal agent;
    HerdComponent comp;
    NavigationAgent3D NavAgent;

    Status status;

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
        status = Status.Running;
        if (comp == null) comp = agent.HerdComponent;
        if (NavAgent == null ) NavAgent = agent.NavAgent;
        
        if (comp == null) GD.PrintErr($"CRAP! Comp is null in MoveHerd:{GetParent().GetPath()}!!!");
    }

    public override void _Exit()
    {
        agent.NavAgent.Velocity = Vector3.Zero;
        agent.Velocity = Vector3.Zero;
    }

    public override Status _Tick(double delta)
    {
        float min_speed = 5.0f;
        float max_speed = 12.0f; 


        Vector3 vel_vect = comp.GetBoidVelocity(delta);

        Vector3 pos_vect = agent.GlobalPosition;
        pos_vect.Y = 0;
        Vector3 new_target = pos_vect + vel_vect;
		NavAgent.TargetPosition = new_target;
        
        if (!NavAgent.IsTargetReachable())
        {  
            Vector3 nearest_point = NavigationServer3D.MapGetClosestPoint(NavAgent.GetNavigationMap(), new_target);
            
            // Calculate direction to the nearest navigable point
            Vector3 dirToSafe = (nearest_point - agent.GlobalPosition).Normalized();
            
            // Blend current velocity with the safe direction
            float blendFactor = 0.5f; // Adjust this to control how quickly agents respond to obstacles
            Vector3 safeVel = dirToSafe * vel_vect.Length();
            vel_vect = vel_vect.Lerp(safeVel, blendFactor);
        }
    
		float speed = vel_vect.Length();

		if (speed > max_speed && speed != 0)
		{
			vel_vect.X = (vel_vect.X/speed) * max_speed;
            vel_vect.Z = (vel_vect.Z/speed) * max_speed;
		}		
		
		if (speed < min_speed && speed != 0)
		{
			vel_vect.X = (vel_vect.X/speed) * min_speed;
            vel_vect.Z = (vel_vect.Z/speed) * min_speed;
		}
        
        
        agent.Velocity = agent.Velocity.Slerp(vel_vect, .07f);

        if ((vel_vect != agent.GlobalPosition) && (vel_vect != Vector3.Zero))
        {
            agent.Rotate(agent.Velocity);
        }

        return status;
    }
    

    public override string[] _GetConfigurationWarnings()
    {
        return Array.Empty<string>();
    }
}
