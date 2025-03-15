using Godot;
using System;
using System.Threading.Tasks;

[Tool]
public partial class  Pounce : BTAction
{
    [Export] float IntialVelocity = 15;
    AnimalHurtBox HurtBox;
    NPCBase PreyFocus;
    Vector3 prey_pos;
    Vector3 prey_vel;
    Vector3 target; 
    Vector3 target_dir;
    float dist_to_target;
    
    Animal agent;
    float speed; 

    public override string _GenerateName()
    {
        return "ApproachAndAttack";
    }

    public override void _Setup()
    {
        if (Agent is Animal agent)
        {
            this.agent = agent;
            speed = agent.RunSpeed;
        }
    }

    public override void _Enter()
    {
        agent.Velocity = Vector3.Zero;
        agent.MoveAndSlide();
        GodotObject focus_obj = (GodotObject)Blackboard.GetVar("PreyFocus"); 
        if (focus_obj is NPCBase npc_obj && IsInstanceValid(npc_obj))
        {
            PreyFocus = npc_obj;
        }
        
        agent.Velocity = Vector3.Zero;
        if (PreyFocus == null) GD.PrintErr("Error -- Preyfocus returned null!!!");
        prey_pos = PreyFocus.GlobalPosition - agent.GlobalPosition; //Local to me
        prey_vel = PreyFocus.Velocity; 
        target = prey_pos;
        target_dir = target.Normalized();

        agent.LookAt(PreyFocus.GlobalPosition);

        float theta = LaunchAngleDeg(target.Length(), IntialVelocity);
        
        GD.PrintErr($"Theta: {theta}");

        if (float.IsNaN(theta))
        {
            GD.PrintErr($"Pounce: Theta is not a number! Cannot reach target");
        }

        Vector3 dir = -agent.Transform.Basis.Z.Rotated(agent.Transform.Basis.X, Mathf.DegToRad(theta));
        agent.Velocity = dir * IntialVelocity + prey_vel;
    }

    public override void _Exit()
    {
        agent.Velocity = Vector3.Zero;
    }

    public override Status _Tick(double delta)
    {   

        
        agent.Rotate(-agent.Velocity);

        if (agent.HitBoxComponent.OverlapsArea(PreyFocus.HitBoxComponent))
        {
            agent.Attack(PreyFocus.HitBoxComponent);
            Blackboard.SetVar("IsFull" , true);
            return Status.Success;
        }

        if ((dist_to_target - (target - agent.GlobalPosition).Length() < dist_to_target/2) && agent.IsOnFloor() && agent.Velocity.Y < .5)
        {
            return Status.Failure;
        }
    
        return Status.Running;
    }

    public float LaunchAngleDeg(float dist, float vel)
    {
        float theta = 0; 
        theta = (0.5f * Mathf.Asin(9.81f * dist/Mathf.Pow(vel, 2)) * 100f);
        return theta;
    }
    
    public override string[] _GetConfigurationWarnings()
    {
        return Array.Empty<string>();
    }

}

