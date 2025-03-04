using Godot;
using System;

[GlobalClass]
public partial class PlayerData : Resource
{
	Player Instance;
	public Inventory PlayerInventory = new();

}
