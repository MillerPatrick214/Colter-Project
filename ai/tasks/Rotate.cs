using Godot;
using System;
using System.Buffers;

[Tool]
public partial class TrackThreat : BTAction
{   
    Node3D focus;
    public override string _GenerateName()
    {
        return "TrackThreat";
    }

        public override void _Setup()
    {
        focus = null;
    }


    public override Status _Tick(double delta)
    {
        focus = (Node3D)Blackboard.GetVar("Focus");

        if (Agent is NPCBase agent)
        {
            Vector3 target = focus.GlobalPosition;
            target = focus.GlobalPosition - agent.GlobalPosition;
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
        }

        return Status.Running;
    }

}
