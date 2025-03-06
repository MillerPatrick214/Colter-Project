using Godot;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public partial class HerdComponent : Area3D
{
	
	Timer timer;

	Area3D Hearing;
	Area3D Sight;

	bool awaitingResponse = false;
	public NPCBase Parent;

	[Export]
	public StringName SpeciesGroup;					//make a group for each species that will filter what species is being added to the list;

	bool CanRequestAddToHerd = true;

	public Godot.Collections.Array<HerdComponent> Herd; //Arrays are always passed by reference btw
	public override void _Ready()
	{
		timer = new Timer();
		NPCBase Parent = (NPCBase)GetParent<Node3D>();

		Hearing = Parent.HearingArea;
		Sight = Parent.VisionCone;

		Herd.Add(this);

		Hearing.AreaEntered += (area) => HerdManagerRequest(area);
		Sight.AreaEntered += (area) => HerdManagerRequest(area);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	

	public void AddToHerd(HerdComponent extern_comp)
	{
		if (extern_comp.IsInGroup(SpeciesGroup))
		{
			Herd.Add(extern_comp);
		}
	}

	public void LeaveHerd()
	{
		Herd = null;
		Herd = new Godot.Collections.Array<HerdComponent>();
	}

	public void TransferToHerd(Godot.Collections.Array<HerdComponent> NewHerd)
	{
		this.Herd = NewHerd;
	}

	public void HerdManagerRequest(Area3D extern_comp)
	{
		if (!CanRequestAddToHerd) {return;}

		HerdComponent ExternalherdComp = extern_comp as HerdComponent;
		if (ExternalherdComp.IsInGroup(SpeciesGroup) && !Herd.Contains(ExternalherdComp))
		{
			HerdManager.Instance.EmitSignal(HerdManager.SignalName.HerdRequest, new HerdRequestTicket(this, ExternalherdComp));
			CanRequestAddToHerd = false;
			timer.Start(3);
			timer.Timeout += () => CanRequestAddToHerd = true;
		}
	}
}

public partial class HerdRequestTicket : Resource	//Request to add a specified NPC to your herd. Keeping this a class as opposed to a struct as the godot array in HerdManager Can't deal with non-variant derived objects :(
{	
	
	public HerdComponent HerdComp;
	public HerdComponent ExternalComp;
	public HerdRequestTicket(HerdComponent HerdComp, HerdComponent ExternalComp) 
	{
		this.HerdComp = HerdComp; this.ExternalComp = ExternalComp;
	}
}
