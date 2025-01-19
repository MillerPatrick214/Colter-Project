using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class DeerSkinTest : Skinnable
{

	Area2D SkinArea; 
	Marker2D StartMarker;
	public override async void _Ready()

	{
		base._Ready();
		SkinArea = GetNodeOrNull<Area2D>("SkinArea");
		StartMarker = GetNodeOrNull<Marker2D>("StartMarker");
		
		//GD.Print((SkinArea != null) ? "" : "DeerSkinTest: SkinArea node returned null");	

		SkinArea.MouseEntered += () => EmitSignal(SignalName.MouseOnSkin, true);
		SkinArea.MouseExited += () => EmitSignal(SignalName.MouseOnSkin, false);
		GetViewport().SizeChanged += ResizeChildren;
		//GD.PrintErr("DeerSkinTest -- Waiting 1 second until we resize children");
		await Task.Delay(1000);		//awaits 1 sec until size correct so we can correctly scale children. This is a temp measure and needs to be adjusted. 	<--------FIXME
		ResizeChildren();
		//GD.PrintErr("DeerSkinTest -- Done resetting children");
	}

	public void ResizeChildren() {				//this function is based off current assumption that we will be using a texturerect that instantiates inside of a ratiocontainer. Since the children of this node aren't control nodes, we must manually resize them.

		Vector2 ScalingFactor = GetSize()/GetMinimumSize();		//Compares rectangle Size to the min size 
		Rect2 rect = GetRect();
		Vector2 center_point = rect.GetCenter();				//Look chase, faggy ass snake case. Are you happy? Do you understand now that your actions have consequences? 

		//GD.PrintErr($"From DeerSkinTest -- Scaling Factor : {ScalingFactor}");
		//GD.PrintErr($"From DeerSkinTest -- Parent TextureRect Position : {Position}");
		Vector2 offset_point = center_point - Position;

		SkinArea.Position = offset_point;
		StartMaker.Position = offset_point - new Vector2(0, rect.Size.Y/4); //new Vector offsets marker 25% the total size of the rect above the center point. 3/4 of the way up.

		SkinArea.Scale = ScalingFactor;
		StartMaker.Scale = ScalingFactor;
	}
}

