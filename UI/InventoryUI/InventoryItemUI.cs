using Godot;
using System;

public partial class InventoryItemUI : TextureRect
{
	// Called when the node enters the scene tree for the first time.


	public static PackedScene DefaultScene = GD.Load<PackedScene>("uid://bvgfu7cifbbyl");

	public InventoryItem item;
	bool is_mouse_on = false;

	bool grabbed = false;

	public override void _Ready()
	{
		ZIndex = 2;
		MouseEntered += () => is_mouse_on = true;
		MouseExited += () => is_mouse_on = false;
	}


	public InventoryItemUI()
	{
		ZIndex = 2;
	}

	public void SetUpitem(InventoryItem item)
	{
		this.item = item;
		ZIndex = 2;
		TooltipText = $"Name:{item.Name}\nDescription:{item.Description}\nValue:{item.Value}";
		Texture = item.Texture;
	}

	public void SetToolTip(InventoryItem item)
	{
		TooltipText = $"Name:{item.Name}\nDescription:{item.Description}\nValue:{item.Value}";
	}

	public void SetTexture(InventoryItem item)
	{
		Texture = item.Texture;
	}

	public override void _Process(double delta)
	{
		Vector2 mouse_pos = GetGlobalMousePosition();

		if (is_mouse_on && Input.IsActionPressed("UseItem") && InvSlotUI.MouseHolding == null)
		{
			grabbed = true;
			Events.Instance.EmitSignal(Events.SignalName.MouseHolding, this);
		}

		if (grabbed)
		{
			MouseDefaultCursorShape = CursorShape.Drag;
			GlobalPosition = mouse_pos;
			if (!Input.IsActionPressed("UseItem"))
			{
				grabbed = false;
				Events.Instance.EmitSignal(Events.SignalName.MouseHolding, null);
			}		
		}

		else if (!grabbed && Position != Vector2.Zero)
		{
			Position = Position.Lerp(Vector2.Zero, 3.0f * (float)delta);
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
}
