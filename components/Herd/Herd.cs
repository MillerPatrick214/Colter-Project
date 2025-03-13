using Godot;
using System;
/*

/// <summary>
/// Herd class that holds array of members, a max size, and calculates herd positions, movement and behavior
/// </summary>
public partial class Herd : Resource

{ 
	public int MaxSize;
	public Vector3 CompSumPosition;

	public Vector3 LastMoveVector = new Vector3(0, 0, -1);
	public Godot.Collections.Array<HerdComponent> HerdArray;

	public bool LeaderHasMoved = false; //set to true when a "leader" animal has moved beyond the herd radius. This forces a readjustment for every animals next move. Calculated move must be within the herd radius.

	public Quaternion CompSumOrientation;

	public Herd(int max_size)
	{
		HerdManager.Instance.EmitSignal(HerdManager.SignalName.HerdCreated, this);
		MaxSize = max_size;
		HerdArray = new();
	}

	public Herd(int max_size, Godot.Collections.Array<HerdComponent> herd_array)
	{
		HerdManager.Instance.EmitSignal(HerdManager.SignalName.HerdCreated, this);
		this.HerdArray = herd_array;
		MaxSize = max_size;
	}
	public Herd()
	{
		HerdManager.Instance.EmitSignal(HerdManager.SignalName.HerdCreated, this);
		MaxSize = 8;
		HerdArray = new();
	}

	/// <summary>
	/// Exported as a Vector 3 so as to avaoid confusion
	/// </summary>
	/// <returns></returns>
	public Vector3 GenStartSumPosition()
	{
		float total_x = 0;
		float total_y = 0;
		float total_z = 0;
		int count = HerdArray.Count;
 
		foreach (HerdComponent comp in HerdArray)
		{
			total_x += comp.GlobalPosition.X;
			total_y += comp.GlobalPosition.Y;
			total_z += comp.GlobalPosition.Z;
		}

		Vector3 vect = new Vector3(total_x, total_y, total_z);
		return vect;
	}

	/// <summary>
	/// Called by individual HerdComponents to shift the herd mean.
	/// </summary>
	/// <param name="comp"></param>
	public void UpdateIndvCompPosition(HerdComponent comp)
	{
		Vector3 tempSum = CompSumPosition - comp.LastPosition;
		CompSumPosition = CompSumPosition + comp.GlobalPosition;
		Vector3 move_vect = comp.GlobalPosition - comp.LastPosition;
		move_vect.Y = 0;
		LastMoveVector = move_vect.Normalized();				//sets direction of last move
	}

	public Vector3 GetMeanPosition()
	{
		int count = HerdArray.Count;
		float scalar = 1/count;

		Vector3 vect = scalar * CompSumPosition;
		return vect;
	}
	

	/// <summary>
	/// Used to add a single component to the Herd
	/// </summary>
	/// <param name="new_comp"></param>
	/// <exception cref="Exception"></exception>
	public void AddToHerd(HerdComponent new_comp)
	{
		if (HerdArray.Count >= MaxSize) {throw new Exception($"Error in Herd Object Path:{this.GetPath()}! Herd is already at MaxSize");}
		HerdArray.Add(new_comp);
	}

	
	/// <summary>
	/// Override for the + operator. Will combine herds only if the overall size is less than the max count of the largest count limit of the two herds. Otherwise will throw exception
	/// </summary>
	/// <param name="herd1"></param>
	/// <param name="herd2"></param>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public static Herd operator+ (Herd herd1, Herd herd2)
	{
		int max_size;
		Godot.Collections.Array<HerdComponent> herd_array = new();

		if (herd1.MaxSize >= herd2.MaxSize) {max_size  = herd1.MaxSize;}
		else {max_size = herd2.MaxSize;}


		if (herd1.HerdArray.Count + herd2.HerdArray.Count > max_size)
		{
			GD.PrintErr("Cannot merge the two herds. Their combined count is larger than the max count allowed.");
			throw new Exception($"Error in Herd Operator+ overload! Herds would be over max size if merged!");
		}

		return new Herd(max_size, herd_array);
	}
	
}
*/
