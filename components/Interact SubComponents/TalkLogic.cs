using Godot;
using System;
using DialogueManagerRuntime;

[GlobalClass]
public partial class TalkLogic : InteractLogic
{
	[Export]
	public string DialogueStart = "start";

	[Export]
	public Resource DialougeResource;
	
	// Called when the node enters the scene tree for the first time.
	public override void Interact()
	{
		DialogueManager.ShowDialogueBalloon(DialougeResource, DialogueStart);
	}
}
