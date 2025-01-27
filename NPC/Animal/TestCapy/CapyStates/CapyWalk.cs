using Godot;
using System;
using System.Numerics;
using System.Threading.Tasks;

public partial class CapyWalk : NPCState<Capybara>
{	
	bool enterComplete = false;


	public override async void _Ready()
	{
		base._Ready();
		
		await AwaitNavAgent();

		if (NPC.NavAgent == null)
		{
			GD.PrintErr("NavAgent is not assigned.");
			return;
		}
		
	}

	public override void Enter(string previousStatePath)
    {
		NPC.NavAgent.NavigationFinished += NavFinished;
		GD.Print("Entering enter function in CapyWalk now... See if I'm Before or After ");
		NPC.AniTree.Set("parameters/conditions/isWalking", true);
		//GD.Print ("Anitree isWalking set to TRUE");

		Godot.Vector3 newRandLocation = new Godot.Vector3(0,0,0);
		Random rnd = new Random();

		newRandLocation.X = rnd.Next(-100, 100) + NPC.GlobalPosition.X;
		newRandLocation.Z = rnd.Next(-100, 100) + NPC.GlobalPosition.Z;

		GD.Print("Before bounds check: ", newRandLocation);

		newRandLocation = NavigationServer3D.MapGetClosestPoint(NPC.NavAgent.GetNavigationMap(), newRandLocation);

		GD.Print("After bounds check: ", newRandLocation);

		NPC.NavAgent.TargetPosition = newRandLocation;
		//NPC.setAnimation("CapybaraAnimations/CapyWalk");
		enterComplete = true;
		
	}

	public override void PhysicsUpdate(double delta)
	{
		if (enterComplete) {												//Very cheesy for now. I think I need to bake flags into base classes or better yet send signals up to state machine for exit/enter to ensure animations are complete etc.
			Godot.Vector3 destination = NPC.NavAgent.GetNextPathPosition();
			Godot.Vector3 LocalDestination = destination - NPC.GlobalPosition;
			
			Godot.Vector3 direction = LocalDestination.Normalized();

			direction.Y = 0;
			GD.PrintErr(direction);
			
			NPC.Rotate(direction);
		
			NPC.Velocity = direction * NPC.GetWalkSpeed();

			NPC.MoveAndSlide();
		
			if (!NPC.IsOnFloor()) {
				EmitSignal(SignalName.Finished, FALL);
			}
		}
	}

	public void NavFinished() 
	{
		GD.Print("FROM WALK STATE: Navigation finished");
		EmitSignal(SignalName.Finished, IDLE);
	}

    public override void Exit()
    {
		NPC.NavAgent.NavigationFinished -= NavFinished;					//if this signal isn't disconnected, I believe NavFinished will be called anytime we reach a nav point thus ending whatever state we're in. This breaks flee mechanic
		NPC.Velocity = Godot.Vector3.Zero; // Reset velocity to zero
    	NPC.MoveAndSlide(); // Apply the reset velocity
		//GD.Print("Exiting CapyWalk state. Velocity reset to: ", NPC.Velocity);
		NPC.AniTree.Set("parameters/conditions/isWalking", false);
		//GD.Print ("Anitree isWalking set to FALSE");
		enterComplete = false;
		
    }
	

	public bool IsApproxEqualCustom(Godot.Vector3 front, Godot.Vector3 target, float tolerance) 	//not currently using. Custom checker as IsApproxEqual() wasn't working well. I think I was just being a dumbass;
	{
		return front.DistanceTo(target) <= tolerance; 
	}

	private async Task AwaitNavAgent() {					//FIXME: this is a shitty temp HACK to avoid navagent being fucking null for some reason despite already awaiting for NPC _Ready to finish and everything else
		while (NPC.NavAgent == null) {						//This feels wrong and shitty. I am pissed tf off 
			GD.Print("Finding NavAgent in Capy Walk...");	//Go fuck yourself whoever is reading this
			await Task.Delay(250);
		}
		GD.Print("CapyWalk found nav Agent!");
	}
}
