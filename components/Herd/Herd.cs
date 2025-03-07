using Godot;
using System;

/// <summary>
/// Herd class that holds array of members, a max size, and calculates herd positions, movement and behavior
/// </summary>
public partial class Herd : Resource
{ 
	public int MaxSize;
	Vector3 MeanPosition;
	public Godot.Collections.Array<HerdComponent> HerdArray;

	public Herd(int max_size)
	{
		MaxSize = max_size;
		HerdArray = new();
	}

	public Herd(int max_size, Godot.Collections.Array<HerdComponent> herd_array)
	{
		this.HerdArray = herd_array;
		
		MaxSize = max_size;
	}
	public Herd()
	{
		MaxSize = 8;
		HerdArray = new();
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
