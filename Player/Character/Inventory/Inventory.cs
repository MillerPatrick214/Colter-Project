using Godot;
using System;

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

    [Export]
    public Godot.Collections.Array<Godot.Collections.Array<InventoryItem>> InventorySpace = new Godot.Collections.Array<Godot.Collections.Array<InventoryItem>> //This represents physical space in inventory, not weapon slots etc. 
    {
        new Godot.Collections.Array<InventoryItem>{null,null,null,null},
        new Godot.Collections.Array<InventoryItem>{null,null,null,null},
        new Godot.Collections.Array<InventoryItem>{null,null,null,null}
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
        GD.PrintErr("Pickup successfully called");

        for (int i = 0; i < InventorySpace.Count; i++) 
        {
            GD.PrintErr($"Entered For loop i{i}");
            Godot.Collections.Array<InventoryItem> array = InventorySpace[i];
            for (int j = 0; j < array.Count; j++) 
            {
                GD.PrintErr($"Entered For loop j{j}");
                GD.PrintErr("At Array J = ", array[j]);
                if (array[j] == null) 
                {
                    array[j] = item; // Assign the item to the empty slot
                    GD.PrintErr($"Success! Item put in slot # {j}");

                    Events.Instance.EmitSignal(Events.SignalName.InventoryChanged);
                    return;         // Exit after placing the item
                }
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

