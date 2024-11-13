using Godot;
using System;
using System.Numerics;


//Will function as base class for future Directional controller nodes;
// Needs to be directly below parent node(Node3D or other NPC maybe?) representing object in hierarchy 
// Sprite3D as child
//Will dynamically update sprite "face" depending on the magnitude of the player x axis in basis relative to the front x axis of object/NPC basis. 

public partial class DirectionalController : Node
{
	CharacterBody3D CharacterNode; 
	Node3D ParentNode;

	Godot.Vector3 ForwardForSelf;
	Godot.Vector3 ForwardForPlayer;

	Sprite3D sprite;
	public override void _Ready()
	{	
		ParentNode = GetParent<Node3D>(); 
		GetPlayerNode();
		sprite = ParentNode.GetNodeOrNull<Sprite3D>("Sprite3D"); 	// there is probably a better way to do this w/ signals -- I need to fully make this a base class and set it up in the most clean way possible.

		if (sprite == null) {
			GD.Print ("Directional Controller: Sprite Node returned null");
		}

	}

	public override void _Process(double delta)
	{
		SetTexture(GetTheta());

	}

	public void GetPlayerNode(){
	
		var nodes = GetTree().GetNodesInGroup("PlayerCharacter"); 	// The type retrieved is really weird

		if (nodes.Count > 0)
		{
			CharacterNode = nodes[0] as CharacterBody3D;			// will always work as I only ever have 1 characterBody3D in the group
		}
		else
		{
			GD.Print("No player character found in the group.");
		}
	}

	public double GetTheta(){										//Linear Algebra classes coming in clutch
		ForwardForPlayer = VectorToCharacter(); 					// cos theta = (dotproduct uv)/(magnitude of U  x  mag of V)
		ForwardForSelf = ParentNode.Transform.Basis.X;

		Godot.Vector2 TwoDPlayerVector = new Godot.Vector2(ForwardForPlayer.X, ForwardForPlayer.Z); //Using Z not Y as Z and X represent the Horizontal/"Floor" plane 
		Godot.Vector2 TwoDForwardVector = new Godot.Vector2(ForwardForSelf.X, ForwardForSelf.Z);	// We don't care about the difference between Y coords from the player to the 

		double DotProduct = TwoDPlayerVector.Dot(TwoDForwardVector);	// I think we need to convert these vectors into vector2 -- no reason to use a 3d vector when we're only calculating on a 2d plane; just fucks it up
		double MagnitudeU = CalculateMagnitude(TwoDPlayerVector);
		double MagntitudeV = CalculateMagnitude(TwoDForwardVector);
		double MagnitudeProduct = MagnitudeU * MagntitudeV;

		double theta = Mathf.Acos(DotProduct/MagnitudeProduct);

		if (ForwardForPlayer[1] >= 0) {
			theta =  (2 * Mathf.Pi) - theta;
		}
		//theta *= 100; //converts into readible decimal
		

		GD.Print("Theta: " + theta);


		return theta;
	}
	
	public double CalculateMagnitude(Godot.Vector2 vector) {
		double magnitude = 0;
		magnitude += Mathf.Pow(vector[0], 2); //Represents X
		magnitude += Mathf.Pow(vector[1], 2); //Represents new Y ("Z" from OG Vect3.)

		magnitude = Mathf.Sqrt(magnitude);

		return magnitude;
	}

	public Godot.Vector3 VectorToCharacter() {
		Godot.Vector3 VectorToCharacter = CharacterNode.GlobalPosition - ParentNode.GlobalPosition; // I think this needs to be the negative vector of the item vector - the player posiion vector  -- basically need to invert the vector we draw to represent the player perspective I think. 
		GD.Print(VectorToCharacter);
		return VectorToCharacter;
	}
	public void SetTexture(double theta){
		
		if ((theta >= 337.5 && theta <= 360) || (theta >= 0 && theta < 22.5)) { //FRONT FACING PLAYER (PROBABLY SHOULD BE REVERSED BUT THIS IS A TEST)
    		sprite.Texture = GD.Load<CompressedTexture2D>("res://Items/TestDirectionalSprite/Textures/Anvil Front.png-394270c90ec8a5521eadcacc9083ebcb.ctex");
			return;
		}
		else if (22.5 <= theta && theta < 67.5) { //FACING NORTH EAST RELATIVE TO PLdwYER
			sprite.Texture = GD.Load<CompressedTexture2D>("res://Items/TestDirectionalSprite/Textures/Anvil North East Temp.png-cd490386e629cb5669903cdf10885770.ctex");
			return;
		}

		else if (67.5 <= theta && theta < 112.5 ) { //RIGHT
			sprite.Texture = GD.Load<CompressedTexture2D>("res://Items/TestDirectionalSprite/Textures/Anvil Right.png-b583ec8f50fbbda728286f1d82a46be8.ctex");
			return;
		}

		else if (112.5 <= theta && theta < 157.5) { //SOUTH EAST
			sprite.Texture = GD.Load<CompressedTexture2D>("res://Items/TestDirectionalSprite/Textures/Anvil South East.png-63b6787ab9cb5be296798300e6400a29.ctex");
			return;
		}

		else if (157.5 <= theta && theta < 202.5) { //BACK 
			sprite.Texture = GD.Load<CompressedTexture2D>("res://Items/TestDirectionalSprite/Textures/Anvil Back.png-66019151d15e8e5ec724705421db184d.ctex");
			return;
		} 

		else if (202.5 <= theta && theta < 247.5) { //SOUTH WEST 
			sprite.Texture = GD.Load<CompressedTexture2D>("res://Items/TestDirectionalSprite/Textures/Anvil South West.png-15622de21864fdd338f972c9a04cf3fa.ctex");
		}

		else if (247.5 <= theta && theta < 292.5){ //LEFT
			sprite.Texture = GD.Load<CompressedTexture2D>("res://Items/TestDirectionalSprite/Textures/Anvil Left.png-d1b37c5d646ec0a1e00295b5b68b89b4.ctex");
		}

		else if (292.5 <= theta && theta < 337.5) { //NORTH WEST 
			sprite.Texture = GD.Load<CompressedTexture2D>("res://Items/TestDirectionalSprite/Textures/AnvilNorthWestTemp.png-fe5f0ccb6e68ab4ed00ec8b9370c89f6.ctex");
		}
	}
}
