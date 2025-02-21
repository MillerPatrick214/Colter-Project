using Godot;
using System;


public partial class CamPivot : Marker3D
{
	[Export]	
	float DefaultFOV = 70;

	[Export]
	float AimFOV = 50;

	bool isInteracting;

	bool isAiming; 

	Camera3D Camera ;


	public override void _Ready()
	{	
		isInteracting = false;
		isAiming = false;		
		Camera = GetNodeOrNull<Camera3D>("Camera3D");
	}

	public override void _Process(double delta) { 
		if (!isInteracting) {
			Aiming(isAiming, delta);

			if (Input.IsActionJustPressed("Aim")) { //Switched the Just Pressed as I only want 1 signal
				isAiming = true;
				Events.Instance.EmitSignal(Events.SignalName.ChangeIsAiming, isAiming);
				//GD.Print("Aiming activated");
			}
				
			else if (Input.IsActionJustReleased("Aim")) {
				//GD.Print("Aiming deactivated");
				isAiming = false;
				Events.Instance.EmitSignal(Events.SignalName.ChangeIsAiming, isAiming);
			}
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public void Aiming(bool isAiming, double delta) {
	
		float targetFOV  = isAiming ? AimFOV : DefaultFOV;

		if (Camera.Fov == targetFOV)
		{
			return;
		}		
		float speed = 4.0f;
		
		Camera.Fov = Mathf.Lerp(Camera.Fov, targetFOV, speed * (float)delta);

	}
}


