using Godot;
using System;
using System.Collections.Generic;

[GlobalClass] 
public partial class PlayerInventory : Resource 
{
    [Export]
    public Wieldable inHands {get; set;} = null;      //so Weapon will be used to represent all holdable objects, including utility for now.
    

    public List<List<int>> InventorySpace = new List<List<int>> //This represents physical space in inventory, not weapon slots etc. 
    {
        new List<int>{0,0,0,0,0},
        new List<int>{0,0,0,0,0},
        new List<int>{0,0,0,0,0}
    };
    
    public List<WieldInvSlot> WieldSlotList = new List<WieldInvSlot> {
        new WieldInvSlot(null, Wieldable.WieldSlot.PrimaryWeapon),
        new WieldInvSlot(null, Wieldable.WieldSlot.PrimaryWeapon),
        new WieldInvSlot(null, Wieldable.WieldSlot.SideArm),
        new WieldInvSlot(null, Wieldable.WieldSlot.SideArm),
        new WieldInvSlot(null, Wieldable.WieldSlot.SideArm),
        new WieldInvSlot(null, Wieldable.WieldSlot.Melee),
        new WieldInvSlot(null, Wieldable.WieldSlot.Utility)
    };



    public List<ClothingSlot> WearableSlotList;
}


public partial class ClothingSlot                       //no reason to have these inherit resource
{
    public Wearable EquipedWearable{get; set;}
    public Wearable.WearSlot WearSlotType{get; set;} = Wearable.WearSlot.NOTSET;
    public ClothingSlot(Wearable EquipedWearable, Wearable.WearSlot WearSlotType) {
        this.EquipedWearable = EquipedWearable;
        this.WearSlotType = WearSlotType;
    }
}

public partial class WieldInvSlot 
{
    public Wieldable ItemInSlot {get; set;}
    public Wieldable.WieldSlot WieldSlotType {get; set;} = Wieldable.WieldSlot.NOTSET;
    public WieldInvSlot(Wieldable ItemInSlot, Wieldable.WieldSlot WieldSlotType) {
        this.ItemInSlot = ItemInSlot;
        this.WieldSlotType = WieldSlotType;
    }
}



