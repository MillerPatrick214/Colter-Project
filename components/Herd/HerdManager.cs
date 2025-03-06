using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class HerdManager : Node
{
	[Signal]
	public delegate void HerdRequestEventHandler(HerdRequestTicket ticket);

	public static HerdManager Instance;
	Godot.Collections.Array<HerdRequestTicket> RequestQueue;

	public override void _Ready()
	{
		Instance ??= this; 
		if (Instance != null) {this.QueueFree();}

		HerdRequest += (ticket) => RequestQueue.Add(ticket); //HerdComponent Asking Mom if it's ok if their friend spends the night basically.

	}

    public override void _Process(double delta)
    {
		if ((RequestQueue.Count == 0)) {return;}
		HerdTicketManager(RequestQueue[0]);		//Return first element in RequestQueue
		

    }

    public void HerdTicketManager(HerdRequestTicket ticket)
	{

		HerdComponent sender_comp = ticket.HerdComp;
		HerdComponent external_comp = ticket.ExternalComp;
		int count = RequestQueue.Count;

		if (count == 1)
		{
			sender_comp.AddToHerd(external_comp);
			external_comp.TransferToHerd(sender_comp.Herd);
			return;
		}

		Godot.Collections.Array<HerdRequestTicket> ToRemove;
		
		for (int i = 1; i < RequestQueue.Count; ++i)
		{
			/*
			HerdRequestTicket newTicket = RequestQueue[i];
			if (sender_comp == newTicket.ExternalComp)
			{

			}
			*/


		}
	}

	

		//if herd is max size return
		
		//Check if NPC matches our parents group
		//Check if We have tickets with shared references either target or sender
			//Tickets with 
			//Compare and see which herd is larger
				//if herd isn't max size...
				//smaller into larger

	}

	

