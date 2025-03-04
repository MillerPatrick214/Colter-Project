using Godot;
using System;

[Tool]
public partial class NewWanderPosition : BTAction
{
    public override string _GenerateName()
    {
        return "NewWanderPosition";
    }

    public override void _Setup()
    {
    }

    public override void _Enter()
    {
        if (Agent is NPCBase agent)
        {
            //agent.NavAgent.NavigationFinished += NavFinished;
		    GD.Print("Entering enter function in CapyWalk now... See if I'm Before or After ");
		    //GD.Print ("Anitree isWalking set to TRUE");

		    Godot.Vector3 newRandLocation = new Godot.Vector3(0,0,0);
		    Random rnd = new Random();

		    newRandLocation.X = rnd.Next(-100, 100) + agent.GlobalPosition.X;
		    newRandLocation.Z = rnd.Next(-100, 100) + agent.GlobalPosition.Z;

		    GD.Print("Before bounds check: ", newRandLocation);

		    newRandLocation = NavigationServer3D.MapGetClosestPoint(agent.NavAgent.GetNavigationMap(), newRandLocation);

		    GD.Print("After bounds check: ", newRandLocation);

		    agent.NavAgent.TargetPosition = newRandLocation;        
        }
    }

    public override void _Exit()
    {
    }

    public override Status _Tick(double delta)
    {

        return Status.Success;
    }

    public override string[] _GetConfigurationWarnings()
    {
        return Array.Empty<string>();
    }
}
