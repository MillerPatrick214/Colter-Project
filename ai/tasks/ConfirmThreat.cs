using Godot;
using System;

[Tool]
public partial class ConfirmThreat : BTAction
{
    float Susometer;
    public override string _GenerateName()
    {
        return "ConfirmThreat";
    }

    public override void _Setup()
    {
    }

    public override void _Enter()
    {
        Susometer = 50;
    }

    public override void _Exit()
    {
    }

    public override Status _Tick(double delta)
    {
        
        if (Agent is NPCBase agent)
        {
            //ROTATION
            //-----------------------------------------------------------------------------------------
            Node3D focus = (Node3D)Blackboard.GetVar("Focus");
            Vector3 direction = (focus.GlobalPosition - agent.GlobalPosition);

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

		    Godot.Quaternion interpolatedQuat = aQuat.Slerp(bQuat, .5f);
		
		    if (aQuat.IsEqualApprox(bQuat)) {		
			    transform.Basis = new Basis(bQuat);
			    agent.Transform = transform;
			    return Status.Success;
		    }

		    transform.Basis = new Basis(interpolatedQuat);
            agent.Transform = transform;
            //-----------------------------------------------------------------------------------------

            //RAYCAST
            //-----------------------------------------------------------------------------------------
            Godot.Vector3 localDir = agent.VisionRay.ToLocal(agent.GlobalPosition +  direction);
            agent.VisionRay.TargetPosition = localDir;

            GodotObject coll_object = agent.GetRayCollision();

            CharacterBody3D char_body = coll_object as CharacterBody3D;
            
            if (char_body != null && char_body.IsInGroup("ThreatLevel3"))
            {
                Susometer += 5 * (float)delta;
            }
            else
            {
                Susometer -= 1 * (float)delta;
            }

            if (Susometer >= 100)
            {
                return Status.Success;
            }
            if (Susometer <= 0)
            {
                return Status.Failure;
            }
        }

        return Status.Running;
    }

    public override string[] _GetConfigurationWarnings()
    {
        return Array.Empty<string>();
    }
}
