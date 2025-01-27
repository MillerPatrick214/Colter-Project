using Godot;
using System;
using System.Threading.Tasks;

public partial class EquipVislMesh : Node
{

	public override async Task _Ready()
	{
		await ToSignal(Owner, SignalName.Ready);
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
