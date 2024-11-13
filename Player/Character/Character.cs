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

	float StandingHeight = 1.7f; //meters
	float CrouchingHeight = 1.0f; //meters

	float StandingCamPivot = 0.195f;
	float CrouchingCamPivot = 0f;

	float LeanDeg = 45f;
	float LeanSpeed = 3f;

	float mouseRotX = 0f;
	float mouseRotY = 0f;
	float lookAroundSpeed = .5f;

	
	float yRotMin = -70f;
	float yRotMax = 70f;

	InteractEvent InteractionEventNode;
	CamPivot CamPivNode;
	TestDoubleBarrel ShotGun;
	InteractRayCast InteractRay;
	CollisionShape3D CollisionShapeNode;

	bool isCrouching;

	bool isInteracting;

	UI UINode;

	GodotObject ObjectSeen; 

	CapsuleShape3D CapsuleShape;	// We need to access the Shape property of our collisionshape3d and store it here 
	public enum LeanDirection {
		Left = 1,
		None = 0,
		Right = -1,

	}

	LeanDirection Leaning;
	
    public override void _Ready()
    {
		Leaning = LeanDirection.None;
		ObjectSeen = null;

		InteractRay = GetNodeOrNull<InteractRayCast>("CamPivot/InteractRayCast");
		ShotGun = GetNodeOrNull<TestDoubleBarrel>("CamPivot/ItemMarker/TestShotGun");	//FIXME -- WARNING: THIS SHOULD LIKELY BE CHANGED TO THE ITEM MARKER IN THE FUTURE. PASS INTERACTION THERE AND THEN TO THE INDIVIDUAL WEAPON;
		CamPivNode = GetNodeOrNull<CamPivot>("CamPivot");
		UINode = GetNodeOrNull<UI>("UI");
		InteractionEventNode = GetNodeOrNull<InteractEvent>("UI/InteractEvent");
		CollisionShapeNode = GetNodeOrNull<CollisionShape3D>("CollisionShape3D");

		CapsuleShape = CollisionShapeNode.Shape as CapsuleShape3D;

		InteractionEventNode.PauseMouseInput += InteractionPause; //signal from Interaction Event node -- processes interact controls. This is a control node and should probably be changed.
		InteractRay.InteractableScan += LookingAtInteract; //signal from raycast that tells what we are looking at
    }

	public override void _Process(double delta) {
		isCrouching = (Input.IsActionPressed("Crouch")) ? true : false;
		//IsLeaning = LeanDirection.None
		
		if (Input.IsActionPressed("LeanLeft")) { Leaning = LeanDirection.Left;}			//These determine Lean Direction based on input.
		else if(Input.IsActionPressed("LeanRight")) { Leaning = LeanDirection.Right;}
		//else if (Input.IsActionPressed("LeanRight") && Input.IsActionPressed("LeanRight")) { IsLeaning = LeanDirection.None;}
		else {Leaning = LeanDirection.None;}
		
		//GD.Print(Leaning);
		Lean(Leaning);
		Crouch(isCrouching);
	}

	public void InteractionPause(bool isActive) { 
		isInteracting = isActive;
		ShotGun.ChangeIsInteracting(isActive);			//FIXME -- this needs to be changed to the node holding the shotgun -- NOT direct to the shotgun as the weapon will change and it will break! 
	}

	public void LookingAtInteract(GodotObject seenObject) {
		GD.Print($"Looking at {seenObject}");
		ObjectSeen = seenObject; 
		UINode.SetInteractable(seenObject);

	}

	public void Crouch(bool isCrouching) {
		float currentHeight = CapsuleShape.Height;
		float currentCamPos = CamPivNode.Position.Y;

		float targetHeight = (isCrouching) ? CrouchingHeight : StandingHeight;
		float targetCamPos = (isCrouching) ? CrouchingCamPivot : StandingCamPivot;
		
		if  (currentHeight != StandingHeight || currentHeight != CrouchingHeight) {
			CapsuleShape.Height = Mathf.Lerp(currentHeight, targetHeight, 0.05f);
			CamPivNode.Position = new Vector3(CamPivNode.Position.X, Mathf.Lerp(currentCamPos, targetCamPos, 0.05f), CamPivNode.Position.Z); 

			if (Math.Abs(targetHeight - currentHeight) < 0.01f) {
				CapsuleShape.Height = targetHeight;
				CamPivNode.Position = new Vector3(0, targetCamPos, 0);

			}
		}
	}

	public override void _Input(InputEvent @event) {
		if (!isInteracting)
			if (@event is InputEventMouseMotion mouseMotion) { //mouseMotion is a local variable here
				// modify accumulated mouse rotation
				mouseRotX += mouseMotion.Relative.X * lookAroundSpeed;		//Note -- The XY may seen flipped, but it's not. Rotation on the X axis is up and down according to the player.
				mouseRotY -= mouseMotion.Relative.Y * lookAroundSpeed;
				
				mouseRotY = Mathf.Clamp(mouseRotY, yRotMin, yRotMax);

				RotationDegrees = new Vector3(RotationDegrees.X, -mouseRotX, RotationDegrees.Z);
				CamPivNode.RotationDegrees = new Vector3(mouseRotY, CamPivNode.RotationDegrees.Y, CamPivNode.RotationDegrees.Z);
			}
	}
	
	public void Lean(LeanDirection Leaning) { //This feels very calculation heavy for something that is called each delta
		
		// Once I clean up the cam pivot node, weapon, etc then I will implement the camera spinning the other way to eliminate disoriantation
		// If cam node and player node share a piv point it just lessens the overall lean subtractivly & disconnects player model & camera

		float currentRotation = RotationDegrees.Z;
		float targetRotation = (float)Leaning * LeanDeg;

		//float currCamPivRot = CamPivNode.RotationDegrees.Z;
		//float targetCamPivRot = -(float)Leaning * (LeanDeg * .5f);		//CamPivot Rotates the opposite direction, x% of the strength of the lean

		float newRotation = Mathf.Wrap(Mathf.Lerp(currentRotation, targetRotation, (float)GetProcessDeltaTime() * LeanSpeed), -LeanDeg-1, LeanDeg+1);	// -+ 1 degree of tolerance
		
		//float newCamPivRot = Mathf.Wrap(Mathf.Lerp(currCamPivRot, targetCamPivRot, (float)GetProcessDeltaTime() * LeanSpeed), -(LeanDeg * .5f)-1, (LeanDeg * .5f)+1);
	
		RotationDegrees = new Vector3(RotationDegrees.X, RotationDegrees.Y, newRotation);
		//CamPivNode.RotationDegrees = new Vector3(CamPivNode.RotationDegrees.X, CamPivNode.RotationDegrees.Y, newCamPivRot);

		if (Math.Abs(targetRotation - currentRotation) < .05f && !(currentRotation == targetRotation)) {		//Checks to see if current rotation is < .5 degrees away from target. If so, just snap to target & return.
			RotationDegrees = new Vector3(RotationDegrees.X, RotationDegrees.Y, targetRotation);
			//having a snap for the CamPivot breaks it, so for the time being we won't have one.
		}

		/* if (Math.Abs(targetCamPivRot - currCamPivRot) < .05f && !(currCamPivRot == targetCamPivRot)) {		//Checks to see if current rotation is < .5 degrees away from target. If so, just snap to target & return.
			CamPivNode.RotationDegrees = new Vector3(CamPivNode.RotationDegrees.X, CamPivNode.RotationDegrees.Y, targetCamPivRot);
			//having a snap for the CamPivot breaks it, so for the time being we won't have one.
		} */

		//GD.Print($"CAM PIVOT: Current Rotation: {currCamPivRot}, Target Rotation: {targetCamPivRot}, New Rotation: {newCamPivRot}");
		//GD.Print($"Current Rotation: {currentRotation}, Target Rotation: {targetRotation}, New Rotation: {newRotation}");
		//A different way of implementing this that might work better w/ game play is Leaning char body more heavily, then rotating cam pivot inversely to a lesser degree. We maintain a small tilt while still exposing hitbox sufficiently

	}
}



