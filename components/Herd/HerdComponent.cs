using Godot;
using System;

public partial class HerdComponent : Area3D
{
	Timer timer;

	Area3D Hearing;
	Area3D Sight;

	bool awaitingResponse = false;
	public NPCBase Parent;

	[Export]
	public StringName SpeciesGroup;					//make a group for each species that will filter what species is being added to the list;

	public Herd Herd;

	bool CanRequestAddToHerd = true;

	public override void _Ready()
	{
		if (SpeciesGroup == null) {GD.PrintErr($"Error {this.GetPath()} No SpeciesGroup set in editor!!!  Will not function.");}
		this.AddToGroup(SpeciesGroup);
		Herd = new();

		timer = new Timer();
		AddChild(timer);

		NPCBase Parent = (NPCBase)GetParent<Node3D>();

		Hearing = Parent.HearingArea;
		Sight = Parent.VisionCone;

		try 
		{
			Herd.AddToHerd(this);
		}
		
		catch (Exception e)
		{
			GD.PrintErr(e);
		}

		Hearing.AreaEntered += (area) => HerdManagerRequest(area);
		Sight.AreaEntered += (area) => HerdManagerRequest(area);

		timer.Timeout += () => CanRequestAddToHerd = true;
	}
	/// <summary>
	/// Attempts to Merge two herds using the operator overload (+) for Herd. Will not merge if combined count over max count limit.
	/// </summary>
	/// <param name="extern_comp"></param>
	public void MergeHerds(HerdComponent extern_comp)
	{
		if (extern_comp.IsInGroup(SpeciesGroup))
		{
			Herd NewHerd;

			try 
			{
				NewHerd = Herd + extern_comp.Herd;
			}
			catch (Exception e)
			{
				GD.PrintErr(e);
				return;
			}
		
			for (int i = 0; i < NewHerd.HerdArray.Count; ++i)
			{
				HerdComponent comp = NewHerd.HerdArray[i];
				comp.Herd = NewHerd;
			}
		}
	}
	
	/// <summary>
	/// Submits ticket to HerdManager singleton to attempt a merge. HerdManager deletes copies of requests. 
	/// </summary>
	/// <param name="extern_comp"></param>
	public void HerdManagerRequest(Area3D extern_comp)
	{
		if (!(extern_comp is HerdComponent)) {return;}
		if (!CanRequestAddToHerd) {return;}

		HerdComponent ExternalherdComp = extern_comp as HerdComponent;
		if (ExternalherdComp.Herd == this.Herd) {return;}

		if (ExternalherdComp.IsInGroup(SpeciesGroup) && !Herd.HerdArray.Contains(ExternalherdComp))
		{
			HerdManager.Instance.EmitSignal(HerdManager.SignalName.HerdRequest, new HerdRequestTicket(this, ExternalherdComp));
			CanRequestAddToHerd = false;
			timer.Start(3);
		}
	}
}

/// <summary>
/// MergeRequest Ticket that contains basic info that the HerdManager needs to sort out requests.
/// </summary>
public partial class HerdRequestTicket : Resource	//Request to add a specified NPC to your herd. Keeping this a class as opposed to a struct as the godot array in HerdManager Can't deal with non-variant derived objects :(
{
	public HerdComponent HerdComp;
	public HerdComponent ExternalComp;
	public HerdRequestTicket(HerdComponent HerdComp, HerdComponent ExternalComp) 
	{
		this.HerdComp = HerdComp; 
		this.ExternalComp = ExternalComp;
	}
}


