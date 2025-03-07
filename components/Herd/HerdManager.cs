using Godot;
using System;

public partial class HerdManager : Node
{
	[Signal]
	public delegate void HerdRequestEventHandler(HerdRequestTicket ticket);

	public static HerdManager Instance;
	Godot.Collections.Array<HerdRequestTicket> RequestQueue;

	public override void _Ready()
	{
		RequestQueue = new();
		if (Instance != null) {this.QueueFree();}
		Instance ??= this; 							// If not null, it's this

		HerdRequest += (ticket) => RequestQueue.Add(ticket); //HerdComponent Asking Mom if it's ok if their friend spends the night basically.

	}

    public override void _Process(double delta)
    {
		if ((RequestQueue.Count == 0)) {return;} //if no requests, chill;
		HerdTicketManager(RequestQueue[0]);		//Return first element in RequestQueue
		

    }

    public void HerdTicketManager(HerdRequestTicket ticket)
	{
		GD.Print("HerdManager Recieved Ticket");
		HerdComponent sender_comp = ticket.HerdComp;
		HerdComponent external_comp = ticket.ExternalComp;
		int count = RequestQueue.Count;

		HerdTransfer(sender_comp, external_comp);

		Godot.Collections.Array<int> IndToRemove = new Godot.Collections.Array<int>();
		
		for (int i = 0; i < RequestQueue.Count; ++i)				
		{
			HerdRequestTicket newTicket = RequestQueue[i];
			
			if (newTicket == ticket || (newTicket.HerdComp == external_comp && newTicket.ExternalComp == sender_comp))		//This also removes the currently handled ticket.
			{
				IndToRemove.Add(i);
			}
		}

		for (int j = 0; j < IndToRemove.Count; ++j)
		{
			int ind = IndToRemove[j];
			RequestQueue.RemoveAt(ind);
		}
	}

	public void HerdTransfer(HerdComponent HerdComp1, HerdComponent HerdComp2)
	{	

		try
		{
			HerdComp1.MergeHerds(HerdComp2);
		}
		catch (Exception e)
		{
			GD.PrintErr(e);
			return;
		}

	}
}

	

