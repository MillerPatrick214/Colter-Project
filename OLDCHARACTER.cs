/* using Godot;
using System;

//I want to implement a state machine here instead of relying on all this crap. 
/*
public partial class Character : CharacterBody3D
{
	[Export]
	public float speed = 14; //so get; set; is a simple way to enable getters/setters w/o a function like we would have to use in CPP -- 9/24/24 did we have get; set;?
	
	float sprintSpeed = 21;
	
	Vector3 targetVelocity = Vector3.Zero; //I hate the stupid _vars. Change this later.
	float verticalComp = 0f;

	float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	float lookAroundSpeed = .5f;

	float mouseRotX = 0f;
	float mouseRotY = 0f;
	
	float yRotMin = -30f;
	float yRotMax = 30f;
	
	float modifiedSpeed;

	Marker3D camPivot;
	AnimatedSprite3D animation;

	public override void _Ready() {
		Input.MouseMode = Input.MouseModeEnum.Captured; 
		camPivot = GetNodeOrNull<Marker3D>("/root/Main/CharacterBody3D/CamPivot");
		animation = GetNode<AnimatedSprite3D>("Pivot/AnimatedSprite3D");
		float modifiedSpeed = speed;
	}

	public override void _Input(InputEvent @event) {
	
		if (@event is InputEventMouseMotion mouseMotion) {
			// modify accumulated mouse rotation
			mouseRotX += mouseMotion.Relative.X * lookAroundSpeed;
			mouseRotY -= mouseMotion.Relative.Y * lookAroundSpeed;
			mouseRotY = Mathf.Clamp(mouseRotY, yRotMin, yRotMax);
		
			this.RotationDegrees = new Vector3(0,-mouseRotX, 0); 		//sets rotation for char and camera separently for x & y.
			camPivot.RotationDegrees = new Vector3(mouseRotY, 0, 0); 
			}

		if (@event is InputEvent InputEventMouseButton) {
			if(Input.IsActionPressed("Aim")) {
				GD.Print("Aiming");
			}
		}

		}
	
	public override void _PhysicsProcess(double delta)
	{
		/* Vector3 direction = Vector3.Zero;
		Vector3 forward = this.GlobalTransform.Basis.Z.Normalized(); //foward direction? This shit is confusing me honestly. The reason we need to do this is so we're always moving relative to the camera 
		Vector3 right = this.GlobalTransform.Basis.X.Normalized(); //Right? 
		
		if (Input.IsActionPressed("Left")) {
			direction -= right;
		}
		if (Input.IsActionPressed("Right")) {
			direction += right;
		}
		if (Input.IsActionPressed("Forward")) {
			direction -= forward;
			
		}
		if (Input.IsActionPressed("Back")) {
			direction += forward;				//Probably can get away with using animatiojn logic here. Should probably keep it simple idk
		}
		
		if (direction != Vector3.Zero) {
			direction = direction.Normalized();
		}
		
		if (Input.IsActionPressed("Sprint")) { 
			targetVelocity = direction * sprintSpeed;
		}

		else {
			targetVelocity = direction * speed;
		}
		
		GD.Print(this.Velocity);
		
		if (!this.IsOnFloor()) {
			verticalComp -= gravity * (float)delta;
			targetVelocity.Y += vrticalComp;
		}e
		else {
			targetVelocity.Y = 0;
			
		}
	
		player.Velocity = targetVelocity;
		
		MoveAndSlide(); */
