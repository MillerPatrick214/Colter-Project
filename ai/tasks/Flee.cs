using Godot;
using System;

[Tool]
public partial class Flee : BTAction
{
    Vector3 threatPosition;
    NPCBase agent;
    Node3D Focus;
    bool Traveling;
    public override string _GenerateName()
    {
        return "Flee";
    }

    public override void _Setup()
    {
        if (Agent is NPCBase agent){this.agent = agent;}
    }

    public override void _Enter()
    {
        Traveling = false;
        Focus = (Node3D)Blackboard.GetVar("Focus");
        
    }

    public override void _Exit()
    {
        Traveling = false;
        agent.Velocity = Vector3.Zero;

    }

    public override Status _Tick(double delta)
    {
	    Vector3 fleeVector = FindFleeVector();
        TravelFleePoint(fleeVector);
        
        if (agent.NavAgent.IsNavigationFinished())
        {
            Traveling = false;
        }
       
        float vectorMagnitude = Mathf.Sqrt(fleeVector.Dot(fleeVector));
	
        if (vectorMagnitude > 50) 
        {				
    	    return Status.Success;
		}
		
        if (agent.NavAgent.IsNavigationFinished())
        {
            Traveling = false;
        }
        return Status.Running;
    }

    public Vector3 FindFleeVector() {					//TLDR; Return vector from threat to NPC
		threatPosition = agent.GetFocus().GlobalPosition;	//by constantly pulling the threat we have threat updating taking place in the character node & have this node react dynamically
		Vector3 fleeDir = agent.GlobalPosition - threatPosition; //Vec A - Vector B results in a vector extending from B to A. AKA from enemy to us.
		//GD.Print("Flee Direction", fleeDir.Normalized()); 
		fleeDir.Y = 0;										//Taking out y component of direction so we have no velocity up or down. Honestly I can probably leave it in here and just remove it for velocity.
		return fleeDir; 						//I'm not returning this as normalized as I'm also using it to look at magnitude. The fleeDir is normalized in FindFleePoint();

	}
    
	public Vector3 FindFleePoint(Vector3 fleeVector) {
		fleeVector = fleeVector.Normalized();
		Vector3 newPosition = agent.GlobalPosition + (fleeVector * 10);				//arbitrary flee point 10 units away 
		//newPosition = NavigationServer3D.MapGetClosestPoint(NPC.NavAgent.GetNavigationMap(), newPosition);
		return newPosition; 
	}
    public void TravelFleePoint(Vector3 fleeVect)
	{
		Vector3 fleePoint;

		if (!Traveling) { //So if navigation is in progress skip finding a new vector and point to flee to.
			fleePoint = FindFleePoint(fleeVect);
			agent.NavAgent.TargetPosition = fleePoint;
		}

		if (!agent.NavAgent.IsTargetReachable())
		{
			RandomNumberGenerator rng = new();
			int randomSign = (rng.RandiRange(0, 1)  > 0) ? 1 : -1; // decides whether or not we will be rotating left or right in case of unreachable target

			for (int i = 0; i < 8; ++i) { //this was a while loop but that was definitely cucking us.
				fleeVect = fleeVect.Rotated(agent.Transform.Basis.Y.Normalized(), randomSign * .25f * Mathf.Pi); //Rotates around LOCAL y axis by 1/8 of a circle, 45 degrees, by a random sign representing left/right rotation1
				fleePoint = FindFleePoint(fleeVect);
				agent.NavAgent.TargetPosition = fleePoint;
				if (!agent.NavAgent.IsTargetReachable())
				{
					continue;
				}
				else
				{
					Traveling = true;
					break;
				}
			}
		}

		if (!Traveling)
		{
			agent.NavAgent.TargetPosition = NavigationServer3D.MapGetClosestPoint(agent.NavAgent.GetNavigationMap(), agent.NavAgent.TargetPosition);
			Traveling = true; 
		}

		Vector3 destination = agent.NavAgent.GetNextPathPosition();
		Vector3 LocalDest = destination - agent.GlobalPosition;
		LocalDest.Y = 0;
		Vector3 direction = LocalDest.Normalized();

		agent.Rotate(direction);
		direction.Y = 0;
		agent.Velocity = direction * agent.GetRunSpeed();
	}

    public override string[] _GetConfigurationWarnings()
    {
        return Array.Empty<string>();
    }

}
