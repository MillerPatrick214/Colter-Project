using Godot;
using System;

public abstract partial class UIElement : Control
{
	[Signal]
	public delegate void DropCurrentControlEventHandler();
	abstract public void Enter();

	abstract public void Exit(); 
}
