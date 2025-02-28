using Godot;
using System;

[Tool]
public partial class Aim : BTAction
{

    Humanoid agent;
    

    public override string _GenerateName()
    {
        return "Aim";
    }

    public override void _Setup()
    {
        if (Agent is Humanoid agent)
        {
            this.agent = agent;
        }
    }

    public override void _Enter()
    {
    }

    public override void _Exit()
    {
    }

    public override Status _Tick(double delta)
    {
        bool hasFocus = (bool)Blackboard.GetVar("hasFocus");
        Node3D focus = agent.GetFocus();
        if (!hasFocus){ return Status.Failure;}

        Status status = Rotate(focus, delta); //Rotates NPC on Y axis only -- visual

        return status;
    }

    public override string[] _GetConfigurationWarnings()
    {
        return Array.Empty<string>();
    }
    
    public Status Rotate(Node3D focus, double delta)
    {
        Vector3 target = focus.GlobalPosition - agent.GlobalPosition;
        target.Y = 0;
        target.Normalized();

        Transform3D transform = agent.Transform;
		 Basis a = agent.Transform.Basis;			

		Basis b =  Basis.LookingAt(-target);

		Godot.Quaternion aQuat = a.GetRotationQuaternion();
		Godot.Quaternion bQuat = b.GetRotationQuaternion();
		aQuat = aQuat.Normalized();
		bQuat = bQuat.Normalized();

		Godot.Quaternion interpolatedQuat = aQuat.Slerp(bQuat, 10f * (float)delta); // faster 2x than rotate that alert
	
		if (aQuat.AngleTo(bQuat) < .25) {   //if within 1/8th of a circle ok to fire
		    transform.Basis = new Basis(bQuat);
		    agent.Transform = transform;
            agent.TEMPFIRE.LookAt(focus.GlobalPosition);
            return Status.Success;
		}

        transform.Basis = new Basis(interpolatedQuat);
        agent.Transform = transform;
        return Status.Running;
    }
}

