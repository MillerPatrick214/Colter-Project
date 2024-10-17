using Godot;
using System;

public partial class Character : CharacterBody3D
{
	InteractEvent InteractionEventNode;
	CamPivot CamPivNode;
	TestDoubleBarrel ShotGun;
    public override void _Ready()
    {
		ShotGun = GetNodeOrNull<TestDoubleBarrel>("CamPivot/ItemMarker/TestShotGun");	//FIXME -- WARNING: THIS SHOULD LIKELY BE CHANGED TO THE ITEM MARKER IN THE FUTURE. PASS INTERACTION THERE AND THEN TO THE INDIVIDUAL WEAPON;
		CamPivNode = GetNodeOrNull<CamPivot>("CamPivot");
		InteractionEventNode = GetNodeOrNull<InteractEvent>("UI/InteractEvent");
		InteractionEventNode.PauseMouseInput += InteractionPause;
    }

	public bool IsInteracting = false;
    [Export]
	public float Speed = 4f;

	[Export]
	public float SprintSpeed = 7f;

	[Export]
	public float JumpImpulse = 100f;

	public void InteractionPause(bool isActive) {
		CamPivNode.ChangeIsInteracting(isActive);
		ShotGun.ChangeIsInteracting(isActive);


	}

}



