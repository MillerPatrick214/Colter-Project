using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Resource 
{
    [Export]
    public Godot.Collections.Array<WieldInvSlot> WieldSlotList = new()
    {
        new WieldInvSlot(null, Wieldable.WieldSlot.PrimaryWeapon),
        new WieldInvSlot(null, Wieldable.WieldSlot.PrimaryWeapon),
        new WieldInvSlot(null, Wieldable.WieldSlot.SideArm),
        new WieldInvSlot(null, Wieldable.WieldSlot.SideArm),
        new WieldInvSlot(null, Wieldable.WieldSlot.SideArm),
        new WieldInvSlot(null, Wieldable.WieldSlot.Melee),
        new WieldInvSlot(null, Wieldable.WieldSlot.Utility)
    };
    
    public List<List<int>> InventorySpace = new List<List<int>> //This represents physical space in inventory, not weapon slots etc. 
    {
        new List<int>{0,0,0,0,0},
        new List<int>{0,0,0,0,0},
        new List<int>{0,0,0,0,0}
    };

    public void SetWieldSlot(Wieldable item, int i) { //i for index
        if (item.EquipSlot == WieldSlotList[i].WieldSlotType) 
        {
            WieldSlotList[i].ItemInSlot = item;     //rn this doesn't check if an item already exists there so it will just boot it out 
        }
        else 
        {
            GD.PrintErr("Player Inventory: Cannot hold weapon in incorrect slot type!");
        }
    }
    /*
    public void SetClothingSlot(Wieldable item, int i) { //i for index
        if (item.EquipSlot != WieldSlotList[i].WieldSlotType) {
            WieldSlotList[i].ItemInSlot = item;
        }
    } */ //not focusing on this yet.
    
    //public List<ClothingSlot> WearableSlotList;\


}

