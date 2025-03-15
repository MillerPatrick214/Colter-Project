using Godot;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/// <summary>
/// BTAction consisting of random, small rotations in order to see if prey is in area 
/// </summary>

[Tool]
public partial class ConfirmPrey : BTAction
{
    [Export]
    int SusSpeed = 70;
    [Export]
    int UnusSpeed = 30;
    float Susometer;
    NPCBase PreyFocus;
    Animal agent;
    public override string _GenerateName()
    {
        return "ConfirmPrey";
    }

    public override void _Setup()
    {
        if (Agent is Animal agent)
        {
            this.agent = agent;
        }
    }

    public override void _Enter()
    {
        GodotObject focus_obj = (GodotObject)Blackboard.GetVar("PreyFocus"); 
        if (focus_obj is NPCBase npc_obj && IsInstanceValid(npc_obj))
        {
            PreyFocus = npc_obj;
        }
        Susometer = 50;
    }

    public override void _Exit()
    {
        Susometer = 50;
    }

    public override Status _Tick(double delta)
    {
        Vector3 direction = PreyFocus.GlobalPosition - agent.GlobalPosition; 
        
        agent.Rotate(-direction);
        SetRayCast(PreyFocus, delta);


        if (Susometer >= 100)
        {
            return Status.Success;
        }
        if (Susometer <= 0)
        {
            return Status.Failure;
        }
        
        return Status.Running;
    }

    public void Rotate(Node3D focus, double delta)
    {
        Vector3 target = focus.GlobalPosition - agent.GlobalPosition;
        if (target == Vector3.Zero) {return;}
        if (!target.IsNormalized()) target = target.Normalized();

        target.Y = 0;
        target.Normalized();
        if (target.IsZeroApprox()) {return;}

        Transform3D transform = agent.Transform;
		 Basis a = agent.Transform.Basis;			

		Basis b =  Basis.LookingAt(-target);

		Godot.Quaternion aQuat = a.GetRotationQuaternion();
		Godot.Quaternion bQuat = b.GetRotationQuaternion();
		aQuat = aQuat.Normalized();
		bQuat = bQuat.Normalized();

		Godot.Quaternion interpolatedQuat = aQuat.Slerp(bQuat, 5f * (float)delta);
	
		if (aQuat.IsEqualApprox(bQuat)) {		
		    transform.Basis = new Basis(bQuat);
		    agent.Transform = transform;
            return;
		}

        transform.Basis = new Basis(interpolatedQuat);
        agent.Transform = transform;
    }

    public void SetRayCast(Node3D focus, double delta)
    {
        Vector3 localDir = agent.VisionRay.ToLocal(agent.GlobalPosition +  PreyFocus.GlobalPosition - agent.GlobalPosition);
        agent.VisionRay.TargetPosition = localDir;

        GodotObject coll_object = agent.GetRayCollision();

        if (coll_object == PreyFocus)
        {
            Susometer += SusSpeed * (float)delta;
        }
        if (coll_object == null)
        {
            Susometer -= UnusSpeed * (float)delta;
        }
    }


    public override string[] _GetConfigurationWarnings()
    {
        return Array.Empty<string>();
    }
}
