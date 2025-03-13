using Godot;

[GlobalClass]
public partial class SkinLogic : InteractLogic
{
	[Export] public string SkinnableScenePath; 
	public override void Interact()
	{
		PackedScene SkinnableScene = GD.Load<PackedScene>(SkinnableScenePath);
		Events.Instance.EmitSignal(Events.SignalName.BeginSkinning, SkinnableScene);
	}
}
