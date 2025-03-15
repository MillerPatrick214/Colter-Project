using Godot;
using System;
using System.Threading;
using System.Threading.Tasks;

[Tool]
public partial class StalkPrey : BTAction
{
    NPCBase PreyFocus;
    Animal agent;
    Vector3 safe_vel;
    [Export] int DistanceToLaunchAttack = 10;

    float speed;

    Status status = Status.Running;

    [Export] int DistanceToAbort = 50;
    public override string _GenerateName()
    {
        return "StalkPrey";
    }

    public override void _Setup()
    {
        if (Agent is Animal agent)
        {
            this.agent = agent;
            speed = agent.RunSpeed;
            agent.NavAgent.VelocityComputed += (vel) => safe_vel = vel;         
            }

        Blackboard.SetVar("DistanceToLaunchAttack", DistanceToLaunchAttack);
        Blackboard.SetVar("DistanceToAbort", DistanceToAbort);
    }

    public override void _Enter()
    {
        status = Status.Running;
    }

    public override void _Exit()
    {
        status = Status.Running;
        agent.Velocity = Vector3.Zero;
    }

    public override Status _Tick(double delta)
    {   
        GodotObject focus_obj = (GodotObject)Blackboard.GetVar("PreyFocus"); 
        if (focus_obj is NPCBase npc_obj && IsInstanceValid(npc_obj))
        {
            PreyFocus = npc_obj;
        }
        
        float distance = (agent.GlobalPosition - PreyFocus.GlobalPosition).Length();
        Blackboard.SetVar("DistanceToPrey", distance);

        if (distance < DistanceToLaunchAttack) return Status.Success;
        if (distance > DistanceToAbort) return Status.Failure;

        Vector3 prey_pos = PreyFocus.GlobalPosition - agent.GlobalPosition; //Local to me
        Vector3 prey_vel = PreyFocus.Velocity;

        Vector3 agent_front_vect = -agent.Transform.Basis.Z.Normalized();
        Vector3 target = (prey_pos + prey_vel);
        
        agent.Rotate(-prey_pos); 

        float dot_factor = agent_front_vect.Dot(prey_vel.Normalized());   // factor represents a factor by which we will incorporate prey velocity into our own
        
        float speed_factor = 1 + dot_factor;    // Max value of 2 if prey is running away, min of 0 if coming directly at us.
        GD.PrintErr($"dot_factor{speed_factor}");   
        Vector3 vel = (prey_pos).Normalized() * speed_factor * speed;
        vel.Y = 0;
        agent.Velocity = vel;

        
        return Status.Running;
    }


    public override string[] _GetConfigurationWarnings()
    {
        return Array.Empty<string>();
    }
}


