using Godot;
using System;

public partial class PlayerInventory : Node
{
	public static PlayerInventory Instance;
	public static Inventory player_inv;
	public override void _Ready()
	{
		Instance = this; 
		player_inv = new Inventory();
	}
}
