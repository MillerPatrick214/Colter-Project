using Godot;
using System;

public partial class Character : CharacterBody3D
{
	[Export]
	public float Speed = 4f;
	[Export]
	public float SprintSpeed = 7f;
	[Export]
	public float JumpImpulse = 100f;

	InteractEvent InteractionEventNode;
	CamPivot CamPivNode;
	TestDoubleBarrel ShotGun;
	InteractRayCast InteractRay;

	UI UINode;

	GodotObject ObjectSeen; 

    public override void _Ready()
    {
		ObjectSeen = null;
		InteractRay = GetNodeOrNull<InteractRayCast>("CamPivot/InteractRayCast");
		ShotGun = GetNodeOrNull<TestDoubleBarrel>("CamPivot/ItemMarker/TestShotGun");	//FIXME -- WARNING: THIS SHOULD LIKELY BE CHANGED TO THE ITEM MARKER IN THE FUTURE. PASS INTERACTION THERE AND THEN TO THE INDIVIDUAL WEAPON;
		CamPivNode = GetNodeOrNull<CamPivot>("CamPivot");
		UINode = GetNodeOrNull<UI>("UI");
		InteractionEventNode = GetNodeOrNull<InteractEvent>("UI/InteractEvent");
		InteractionEventNode.PauseMouseInput += InteractionPause;
		InteractRay.InteractableScan += LookingAtInteract;
    }

	public void InteractionPause(bool isActive) { 
		CamPivNode.ChangeIsInteracting(isActive);
		ShotGun.ChangeIsInteracting(isActive);
	}

	public void LookingAtInteract(GodotObject seenObject) {
		GD.Print($"Looking at {seenObject}");
		ObjectSeen = seenObject; 
		UINode.SetInteractable(seenObject);

	}
}



