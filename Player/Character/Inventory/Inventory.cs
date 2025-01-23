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

    public List<List<InventoryItem>> InventorySpace = new List<List<InventoryItem>> //This represents physical space in inventory, not weapon slots etc. 
    {
        new List<InventoryItem>{null,null,null,null},
        new List<InventoryItem>{null,null,null,null},
        new List<InventoryItem>{null,null,null,null}
    };
    

    public void SetWieldSlot(Wieldable item, int i)  //i for index
    { 
        if (item.EquipSlot == WieldSlotList[i].WieldSlotType) 
        {
            WieldSlotList[i].ItemInSlot = item;     //rn this doesn't check if an item already exists there so it will just boot it out 
        }
        else 
        {
            GD.PrintErr("Player Inventory: Cannot hold weapon in incorrect slot type!");
        }
    }

    public void PickUpItem(InventoryItem item) // Doesn't take space into account
    {
        for (int i = 0; i < InventorySpace.Count; i++) 
        {
            List<InventoryItem> list = InventorySpace[i];
            for (int j = 0; j < list.Count; j++) 
            {
                if (list[j] == null) 
                {
                    list[j] = item; // Assign the item to the empty slot
                    GD.Print($"Success! Item put in slot # {j}");
                    return;         // Exit after placing the item
                }
                else {continue;}
            }
        }
        GD.PrintErr("Inventory Error: No space found for item in inventory.");
    }

    public Wieldable SelectFromSlot(int i)
    {
        WieldInvSlot slot = WieldSlotList[i];
        Wieldable item = slot.ItemInSlot;

        if (item == null) 
        {
            GD.PrintErr("Inventory Error: No Item in slot!");
            return null;
        }

        else 
        {
            return item; 
        }

    }
    public void DropItem(InventoryItem item) 
    {

    }
    
    public void MoveItem(InventoryItem item) 
    {
        
    }
    /*
    public void SetClothingSlot(Wieldable item, int i) { //i for index
        if (item.EquipSlot != WieldSlotList[i].WieldSlotType) {
            WieldSlotList[i].ItemInSlot = item;
        }
    } */ //not focusing on this yet.
    
    //public List<ClothingSlot> WearableSlotList;\
}

