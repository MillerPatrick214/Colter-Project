using Godot;
using System;
using System.Threading.Tasks; //very explicity needs this to be included due to awaying a task delay


public partial class CapyFlee : NPCState<Capybara>
{

	Vector3 threatPosition;
	
	Vector3 fleePosition;

	bool Traveling = false; 


	public override void Enter(string previousState) 
	{
		Traveling = false;
		NPC.NavAgent.NavigationFinished += NavFinished;
		NPC.NavAgent.Use3DAvoidance = true;

	}
	
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		base._Ready();

		await AwaitNavAgent();
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void PhysicsUpdate(double delta)	//This entire Physics update needs to be reworked to enhance performance. I feel like all the Curr AI states are bussed rn;
	{	
		Vector3 fleeVector = FindFleeVector();
		TravelFleePoint(fleeVector);

		if (!Traveling)
		{
			float vectorMagnitude = Mathf.Sqrt(fleeVector.Dot(fleeVector));
			if (vectorMagnitude > 50) {				//if the magnitude(distance) of the vector between our position and the threat's is greater than x amount, chill
				EmitSignal(SignalName.Finished, IDLE);
				return;
			}
		}
		
		if (!NPC.IsOnFloor()) {
			EmitSignal(SignalName.Finished, FALL);
		}
	}

	public Vector3 FindFleeVector() {					//TLDR; Return vector from threat to NPC
		threatPosition = NPC.GetThreat().GlobalPosition;	//by constantly pulling the threat we have threat updating taking place in the character node & have this node react dynamically
		Vector3 fleeDir = NPC.GlobalPosition - threatPosition; //Vec A - Vector B results in a vector extending from B to A. AKA from enemy to us.
		//GD.Print("Flee Direction", fleeDir.Normalized()); 
		fleeDir.Y = 0;										//Taking out y component of direction so we have no velocity up or down. Honestly I can probably leave it in here and just remove it for velocity.
		return fleeDir; 						//I'm not returning this as normalized as I'm also using it to look at magnitude. The fleeDir is normalized in FindFleePoint();

	}

	public Vector3 FindFleePoint(Vector3 fleeVector) {
		fleeVector = fleeVector.Normalized();
		Vector3 newPosition = NPC.GlobalPosition + (fleeVector * 10);				//arbitrary flee point 10 units away 
		//newPosition = NavigationServer3D.MapGetClosestPoint(NPC.NavAgent.GetNavigationMap(), newPosition);
		return newPosition; 
	}

	public void TravelFleePoint(Vector3 fleeVect)
	{
		if (!Traveling) { //So if navigation is in progress skip finding a new vector and point to flee to.
			Vector3 fleePoint = FindFleePoint(fleeVect);
			NPC.AniTree.Set("parameters/conditions/isWalking", true);
			NPC.NavAgent.TargetPosition = fleePoint;

			RandomNumberGenerator rng = new();
			int randomSign = (rng.RandiRange(0, 1)  > 0) ? 1 : -1; // decides whether or not we will be rotating left or right in case of unreachable target
			
			if (!NPC.NavAgent.IsTargetReachable()) { //this was a while loop but that was definitely cucking us.
				fleeVect = fleeVect.Rotated(NPC.Transform.Basis.Y.Normalized(), randomSign * .50f * Mathf.Pi); //Rotates around LOCAL y axis by 1/8 of a circle, 45 degrees, by a random sign representing left/right rotation
				fleePoint = FindFleePoint(fleeVect);
				NPC.NavAgent.TargetPosition = NavigationServer3D.MapGetClosestPoint(NPC.NavAgent.GetNavigationMap(), fleePoint);
			}
			
			Traveling = true; 
		}

		Vector3 destination = NPC.NavAgent.GetNextPathPosition();
		Vector3 LocalDest = destination - NPC.GlobalPosition;
		LocalDest.Y = 0;
		Vector3 direction = LocalDest.Normalized();

		NPC.Rotate(direction);
		direction.Y = 0;
		NPC.Velocity = direction * NPC.GetFleeSpeed();
	}

	public void NavFinished()	//once navagent recognizes that the traveling is complete, set Traveling to false allowing us to find a new point to travel to if necessary
	{
		Traveling = false;	
	}

    public override void Exit()
    {
        base.Exit();
		Traveling = false;
		NPC.AniTree.Set("parameters/conditions/isWalking", false);
		NPC.NavAgent.NavigationFinished -= NavFinished;
    }

	private async Task AwaitNavAgent() {					//FIXME: this is a shitty temp HACK to avoid navagent being fucking null for some reason despite already awaiting for NPC _Ready to finish in the base class and everything else
		while (NPC.NavAgent == null) {						//This feels wrong and shitty. I am pissed tf off 
			GD.Print("Finding NavAgent in Capy Walk...");	//Go fuck yourself whoever is reading this
			await Task.Delay(250);
		}
		GD.Print("CapyWalk found nav Agent!");			
	}
}
