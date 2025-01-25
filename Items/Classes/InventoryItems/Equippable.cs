using Godot;
using System;

[GlobalClass] 
public partial class Equippable : InventoryItem 
{
	
	public enum EquipSlot
    {
        NOTSET,
        Utility,
        PrimaryWeapon,
        SideArm,
        Melee,
        Head,
        Torso,
        Legs,
        Cover,
        Tailsman,
        Feet           
    }

	[Export]
    public EquipSlot SlotType {get; set;}
    
    public Equippable() : this(null, null, EquipSlot.NOTSET){}

    public Equippable(string ScenePath, PackedScene WorldItemScene,  EquipSlot SlotType) {       //keep in mind this is just for instantiating an item into inventory, not actually instantiating the scene in the players hands or anything like that.
        this.ScenePath = ScenePath;
        this.SlotType = SlotType;
    }
}