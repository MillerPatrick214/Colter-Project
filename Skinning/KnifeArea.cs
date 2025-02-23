using Godot;

public partial class KnifeArea : Area2D
{
	[Signal]
	public delegate void MouseOnKnifeEventHandler(bool isTrue);
	bool isMouseOnKnife = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MouseEntered += () => {
			isMouseOnKnife = true;
			EmitSignal(SignalName.MouseOnKnife, true);
		};

		MouseExited += () => {
			isMouseOnKnife = false;
			EmitSignal(SignalName.MouseOnKnife, false);
		};

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}


}
