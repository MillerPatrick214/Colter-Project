using Godot;
using System;

[Tool]
public partial class ConfirmThreat : BTAction
{
    [Export]
    int SusSpeed = 10;
    [Export]
    int UnusSpeed = 5;
    float Susometer;
    NPCBase agent;
    public override string _GenerateName()
    {
        return "ConfirmThreat";
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
        Susometer = 50;
    }

    public override void _Exit()
    {
        Susometer = 50;
    }

    public override Status _Tick(double delta)
    {
        Node3D focus = agent.GetFocus();
        if (focus == null)
        {
            return Status.Failure;
        }

        Vector3 direction = focus.GlobalPosition - agent.GlobalPosition; 

        Rotate(focus, delta);
        SetRayCast(focus, direction, delta);
        
        
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
        target.Y = 0;
        target.Normalized();

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

    public void SetRayCast(Node3D focus, Vector3 direction, double delta)
    {
        Vector3 localDir = agent.VisionRay.ToLocal(agent.GlobalPosition +  direction);
        agent.VisionRay.TargetPosition = localDir;

        GodotObject coll_object = agent.GetRayCollision();

        CharacterBody3D char_body = coll_object as CharacterBody3D;
        
        if (char_body != null && char_body.IsInGroup("ThreatLevel3"))
        {
            Susometer += 5 * (float)delta;
        }
        else
        {
            Susometer -= 3 * (float)delta;
        }
    }

    public override string[] _GetConfigurationWarnings()
    {
        return Array.Empty<string>();
    }
}
