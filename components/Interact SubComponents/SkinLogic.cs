using Godot;
using System;

[GlobalClass]
public partial class SkinLogic : InteractLogic
{
	[Export]
	public string SkinningNodePath; 
	public override void Interact()
	{
		PackedScene SkinningScene = GD.Load<PackedScene>(SkinningNodePath);
		Events.Instance.EmitSignal(Events.SignalName.BeginSkinning, SkinningScene);
	}
}
