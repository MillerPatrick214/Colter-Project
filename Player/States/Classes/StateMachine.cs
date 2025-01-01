using Godot;
using System;

public partial class StateMachine : Node
{	
	State state; 		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		state = GetNodeOrNull<State> ("Idle");
		
		foreach (Node childNode in GetChildren()) {
				if (childNode is State stateNode)
				stateNode.Finished += TransitionToNextState;
		}

		state.Enter("");
	}

	public void TransitionToNextState(String targetStatePath) {								//O1 example has a dict but idk type yet so leaving out. 
		if (!HasNode(targetStatePath)) {													//Also using isMove is kinda a cheesy way to get 2 State machines out of 1. Probably will need to change in the future. Will work for now to avoid dividing this script up too much for just a demo. 
			GD.Print($"{Owner.Name}: Trying to transition to state {targetStatePath} but it does not exist.");
			return;
		}
		
		string previousStatePath = state.Name;
		state.Exit();
		state = GetNode<State>(targetStatePath);
		state.Enter(previousStatePath);
		GD.Print($"{Owner.Name} transitioned from {previousStatePath} to {targetStatePath}."); 
	}

	public void UnhandledInput(InputEvent @event) {
		state.HandleInput(@event);
	}

	public override void _Process(double delta) {
		state.Update(delta);
	}

	public override void _PhysicsProcess(double delta) {
		state.PhysicsUpdate(delta);
		}
}



