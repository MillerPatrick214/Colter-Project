using Godot;
using System;

public partial class InteractText : Control
{
	string BaseText = "'E' to interact with ";
	//InteractRayCast raycast; //Interestingly, it won't allow me to define this as a RayCast3D object without issues -- it won't recognize the signal.
	//CharacterBody3D Char3D;  //Cont from above -> Odd since I haven't had issues with other classes and their signals.
	Control UIParent;
	RichTextLabel label;

	public override void _Ready()
	{
		Hide();
		label = GetNodeOrNull<RichTextLabel>("MarginContainer/InteractWith");
		

	}

	public void Seen(GodotObject InteractableObject) {
		if (InteractableObject != null) {
		GD.Print("Recieved Interactable");
		Show();
		
		string newText = BaseText + (string)InteractableObject.Get("name");
		label.Text = newText;
		GD.Print(newText); 
		}
		else {
			Hide();
		}

	}
}
