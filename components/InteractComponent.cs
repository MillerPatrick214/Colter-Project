using Godot;
using System.Threading.Tasks;


[GlobalClass]
public partial class InteractComponent : Area3D
{
	
	[Export]
	public InteractMode CurrentInteractMode {get; set;}
	[Export]
	CollisionShape3D CollisionShape;
	[Export]
	public SkinLogic SkinLogic;

	[Export]
	public TalkLogic TalkLogic;

	[Export]
	public LootLogic LootLogic;

	[Export]
	public PickUpLogic PickUpLogic;

	public Node ParentNode;

	public enum InteractMode 
	{
		NONE,
		SKIN,
		TALK,
		LOOT,
		PICKUP
	}
	
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		
		if (ParentNode != null){ return;}

    	while (!IsInsideTree())
		{
			await Task.Delay(10); // Small delay to avoid blocking the thread
		}
		ParentNode = GetParent();
	}

	public void Interact()
	{
		switch (CurrentInteractMode)
		{
			case (InteractMode.NONE):
			return;
			
			case (InteractMode.SKIN):
			if (SkinLogic == null) {return;}
			SkinLogic.Interact();
			CurrentInteractMode = InteractMode.NONE;
			return;

			case(InteractMode.TALK):
			if (TalkLogic == null) {return;}
			TalkLogic.Interact();
			return;

			case(InteractMode.LOOT):
			return;

			case(InteractMode.PICKUP):
			if (PickUpLogic == null) {return;}
			PickUpLogic.Interact(ParentNode);
			return;
		}
	}
	/// <summary>
	/// Set InteractComponent's Collision Shape to Disabled (true) or Active (false)
	/// </summary>
	/// <param name="tf"></param>
	public void SetDisabled(bool tf)
	{
		CollisionShape.Disabled = tf;
	}
}


