using Godot;

public partial class TestDeer : NPCBase
{
	[Signal] public delegate void SensedEventHandler();
	PackedScene SkinningScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		SkinningScene = GD.Load<PackedScene>("res://Skinning/DeerSkinTest.tscn");
	}

	private Vector3 _velocity = Vector3.Zero;
	private float _speed = 10.0f;
	private bool isMoving = false;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (isMoving) {
			_velocity = Transform.Basis.Z * -_speed;
		}
		else {
			_velocity = Vector3.Zero;
		}
		_velocity.Y -= (float)(gravity * 4.0f * delta);
		Velocity = _velocity;
		MoveAndSlide();
	}
	private void OnMoveTimerTimeout()
    {
        isMoving = !isMoving; // Toggle movement state
		if (!isMoving) {
			RotateY((float)GD.RandRange(-Mathf.Pi / 2.0, Mathf.Pi / 2.0)); // Rotate randomly
		}
    }

	public override void Death() {
		base.Death(); 
		IsInteractable = true;
	}

	public override void Interact() {
		if (SkinningScene == null) {
			GD.Print("TestDeer: Error, skinning scene came back as null");
		}
		Events.Instance.EmitSignal(Events.SignalName.BeginSkinning, SkinningScene);
	}
}