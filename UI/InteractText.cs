using Godot;
using System;

public partial class InteractText : Control
{
	string BaseText = "'E' to interact with ";
	InteractRayCast raycast; //Interestingly, it won't allow me to define this as a RayCast3D object without issues -- it won't recognize the signal.
	CharacterBody3D Char3D;  //Cont from above -> Odd since I haven't had issues with other classes and their signals.

	Control UIParent;
	RichTextLabel label;

	public override void _Ready()
	{
		Hide();
		Node rootNode = GetTree().Root;
		Char3D = rootNode.GetNode<CharacterBody3D>("TestMap/Character");
		label = GetNode<RichTextLabel>("MarginContainer/InteractWith");
		raycast = Char3D.GetNode<InteractRayCast>("CamPivot/InteractRayCast");
		
		/*
		GD.Print(raycast);
		GD.Print(Char3D); 
		GD.Print(label);
		*/ 

		if (label == null){
			GD.Print("Unable to find label :(");
		}
 		if (Char3D == null){
			GD.Print("Error finding Character node");
		}
		if (raycast == null) {
			GD.Print("Attempted to find raycast script in order to connect signal but returned Null. Have you moved the raycast file dumbass?");
		}
		else {
			GD.Print("Connecting signals...");
			raycast.InteractableSeen += Seen;
			raycast.InteractableLost += Lost;
			GD.Print("Signals connected!");
		}

	}

	public void Seen(GodotObject InteractableObject) {
		GD.Print("Recieved Interactable");
		Show();
		
		string newText = BaseText + (string)InteractableObject.Get("name");
		label.Text = newText;
		GD.Print(newText); 

	}
	public void Lost() {
		Hide();
	}
}
