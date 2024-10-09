using Godot;
using System;

public partial class AnimationNode : Node
{
	// Called when the node enters the scene tree for the first time.

	CharacterBody3D Parent;
	CamPivot CamPivotNode;
	AnimationPlayer AniPlayer;
	public override void _Ready()
	{
		Parent = GetParent<CharacterBody3D>();
		CamPivotNode = Parent.GetNodeOrNull<CamPivot>("CamPivot");

		if (CamPivotNode == null) {
			GD.Print("Animation Node: CamPivotNode returned null. Unable to control item/weapon animations");
		}

		AniPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");

		if (AniPlayer == null) {
			GD.Print("Animation Node: AnimationPlayer returned null (child node)");
		}	


		CamPivotNode.AimSignal += AimingAnimation;



	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void AimingAnimation(bool isAiming) 
	{	
		if (isAiming) {
			AniPlayer.Play("Aiming");
			
		}

		else {
				AniPlayer.PlayBackwards("Aiming");
		}
	}
}
