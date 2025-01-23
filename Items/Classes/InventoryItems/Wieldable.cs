using Godot;
using System;

[GlobalClass] 
public partial class Wieldable : InventoryItem 
{
	
	public enum WieldSlot
    {
        NOTSET,
        Utility,
        PrimaryWeapon,
        SideArm,
        Melee 
    }

	[Export]
    public WieldSlot EquipSlot {get; set;}
    
    public Wieldable() : this(null, null, WieldSlot.NOTSET){}

    public Wieldable(string ScenePath, PackedScene WorldItemScene,  WieldSlot EquipSlot) {       //keep in mind this is just for instantiating an item into inventory, not actually instantiating the scene in the players hands or anything like that.
        this.ScenePath = ScenePath;
        this.EquipSlot = EquipSlot;
    }
}