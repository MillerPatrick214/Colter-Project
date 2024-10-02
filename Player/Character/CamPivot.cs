using Godot;
using System;

public partial class CamPivot : Marker3D
{

	float lookAroundSpeed = .5f;
	float mouseRotX = 0f;
	float mouseRotY = 0f;
	
	float yRotMin = -70f;
	float yRotMax = 70f;
	CharacterBody3D char3D;
	SpringArm3D springArm;

	float DefaultFOV = 70;
	float AimFOV = 50;


	Vector3 CurrentArmPos;

	bool isAiming; 

	Camera3D Camera;

	public override void _Ready()
	{
		isAiming = false;		
		char3D = GetParent() as CharacterBody3D;		//need this as we are rotating the actual character with mousemovement. We only move camera up & down. 
		Input.MouseMode = Input.MouseModeEnum.Captured; 
		Camera = GetNodeOrNull<Camera3D>("Camera3D");
		if (Camera == null) {
			GD.Print("CamPivot: Camera3d returned null");
		}
		
	}

	public override void _Process(double delta) { 
		Aiming(isAiming, delta);

		if (Input.IsActionPressed("Aim")) {
			isAiming = true;
			//GD.Print("Aiming activated");
		}
			
		else if (Input.IsActionJustReleased("Aim")) {
			//GD.Print("Aiming deactivated");
			isAiming = false;
		}

	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion mouseMotion) { //mouseMotion is a local variable here
			// modify accumulated mouse rotation
			mouseRotX += mouseMotion.Relative.X * lookAroundSpeed;
			mouseRotY -= mouseMotion.Relative.Y * lookAroundSpeed;
			mouseRotY = Mathf.Clamp(mouseRotY, yRotMin, yRotMax);
		
			char3D.RotationDegrees = new Vector3(0,-mouseRotX, 0); 		//sets rotation for char and camera separently for x & y.
			this.RotationDegrees = new Vector3(mouseRotY, 0, 0); 
			}
		}

	public void Aiming(bool isAiming, double delta) {
		float currentFOV = Camera.Fov;

		if (isAiming) {
			Camera.Fov = currentFOV + (AimFOV - currentFOV) * 4 *(float)delta;
		}

		if (!(isAiming)) {
			Camera.Fov = currentFOV + (DefaultFOV - currentFOV) * 4 *(float)delta;

		} 
	}
}



