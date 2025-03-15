using Godot;
using System;

[Tool]
public partial class NewWanderPosition : BTAction
{
    Godot.Vector3 newRandLocation = new Godot.Vector3(0,0,0);

    NavigationAgent3D NavAgent;
    NPCBase agent;
    public override string _GenerateName()
    {
        return "NewWanderPosition";
    }

    public override void _Setup()
    {
        if (Agent is NPCBase agent)
        {
            this.agent = agent;
            NavAgent = agent.NavAgent;
        }
    }

    public override void _Enter()
    {
		newRandLocation = new Godot.Vector3(0,0,0);
		Random rnd = new Random();

		newRandLocation.X = rnd.Next(-100, 100) + agent.GlobalPosition.X;     
		newRandLocation.Z = rnd.Next(-100, 100) + agent.GlobalPosition.Z;

		GD.Print("Before bounds check: ", newRandLocation);

		newRandLocation = NavigationServer3D.MapGetClosestPoint(NavAgent.GetNavigationMap(), newRandLocation);

		GD.Print("After bounds check: ", newRandLocation);

		agent.NavAgent.TargetPosition = newRandLocation;     
        
    }

    public override void _Exit()
    {
        Blackboard.SetVar("NavTarget", newRandLocation);
        newRandLocation = new Godot.Vector3(0,0,0);
    }

    public Vector3 PickRandPoint()
    {
        newRandLocation = new Godot.Vector3(0,0,0);
		Random rnd = new Random();

		newRandLocation.X = rnd.Next(-100, 100) + agent.GlobalPosition.X;     
		newRandLocation.Z = rnd.Next(-100, 100) + agent.GlobalPosition.Z;

		GD.Print("Before bounds check: ", newRandLocation);

		newRandLocation = NavigationServer3D.MapGetClosestPoint(NavAgent.GetNavigationMap(), newRandLocation);

		GD.Print("After bounds check: ", newRandLocation);

		NavAgent.TargetPosition = newRandLocation;
        return newRandLocation;
    }

    public override Status _Tick(double delta)
    {
        PickRandPoint();
        if (newRandLocation != Vector3.Zero) return Status.Success;
        GD.PrintErr("Error in NewWanderPosition -- May be stuck in loop where I cannot find a random map value! If you only see me once, disregard");
        return Status.Running; 
    }

    public override string[] _GetConfigurationWarnings()
    {
        return Array.Empty<string>();
    }
}
