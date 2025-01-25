using Godot;
using System;

public partial class Inventory : Resource 
{
    public Inventory()
    {
        GD.PrintErr("Inventory!");
    }

    //[Export] 
    public Godot.Collections.Array<EquipInvSlot> EquippedSlotList = new()
    {
        new EquipInvSlot(null, Equippable.EquipSlot.PrimaryWeapon),
        new EquipInvSlot(null, Equippable.EquipSlot.PrimaryWeapon),
        new EquipInvSlot(null, Equippable.EquipSlot.SideArm),
        new EquipInvSlot(null, Equippable.EquipSlot.SideArm),
        new EquipInvSlot(null, Equippable.EquipSlot.SideArm),
        new EquipInvSlot(null, Equippable.EquipSlot.Melee),
        new EquipInvSlot(null, Equippable.EquipSlot.Utility),
        new EquipInvSlot(null, Equippable.EquipSlot.Head),
        new EquipInvSlot(null, Equippable.EquipSlot.Torso),
        new EquipInvSlot(null, Equippable.EquipSlot.Legs),
        new EquipInvSlot(null, Equippable.EquipSlot.Cover),
        new EquipInvSlot(null, Equippable.EquipSlot.Tailsman),
        new EquipInvSlot(null, Equippable.EquipSlot.Feet)
    };

    [Export]
    public Godot.Collections.Array<Godot.Collections.Array<InventoryItem>> InventorySpace = new Godot.Collections.Array<Godot.Collections.Array<InventoryItem>> //This represents physical space in inventory, not weapon slots etc. 
    {
        new Godot.Collections.Array<InventoryItem>{null,null,null,null},
        new Godot.Collections.Array<InventoryItem>{null,null,null,null},
        new Godot.Collections.Array<InventoryItem>{null,null,null,null}
    };

    public int GetRows()
    {
       return InventorySpace.Count; 
    } 
    public int GetColumns()
    {
        return  InventorySpace[0].Count;

    } 

    public void CheckEquipSlot(Equippable item, int i)  //i for index. Index will be held by the EquipSlotUI object making the call and passed down.
    { 
        if (item.SlotType != EquippedSlotList[i].EquipSlotType) 
        {
            throw(new ArgumentException("Player Inventory: Cannot hold weapon in incorrect slot type!"));
            return;
        }
        else if (EquippedSlotList[i] == null)
        {
            throw(new ArgumentException("Player Inventory: Slot is full!"));
            return;
        }
        else
        {
            EquippedSlotList[i].ItemInSlot = item;
        }
    }

    public void PickUpItem(InventoryItem item) // Doesn't take space into account
    {
        //GD.PrintErr("Pickup successfully called");

        for (int i = 0; i < InventorySpace.Count; i++) 
        {
            //GD.PrintErr($"Inventory: Entered For loop i{i}");
            Godot.Collections.Array<InventoryItem> array = InventorySpace[i];
            for (int j = 0; j < array.Count; j++) 
            {
               // GD.PrintErr($"Inventory: Entered For loop j{j}");
               // GD.PrintErr("At Array J = ", array[j]);
                if (array[j] == null) 
                {
                    array[j] = item; // Assign the item to the empty slot

                    item.row = i;
                    item.column = j;

                    GD.PrintErr($"Success! Item put in slot # {j}");

                    Events.Instance.EmitSignal(Events.SignalName.InventoryChanged);
                    return;         // Exit after placing the item
                }
            }
        }

        GD.PrintErr("Inventory Error: No space found for item in inventory.");
    }

    public Equippable EquipFromSlot(int i)
    {
        EquipInvSlot slot = EquippedSlotList[i];
        Equippable item = slot.ItemInSlot;

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
    
    public void MoveItem(InventoryItem item, int row, int column)   //To Inv slot
    {
        if (item.slot == -1)
        {
            InventorySpace[item.row][item.column] = null;
        }
        else
        {
            EquippedSlotList[item.slot].ItemInSlot = null;
        }
        InventorySpace[row][column] = item;

        item.SetPosition(row, column);

        Events.Instance.EmitSignal(Events.SignalName.InventoryChanged); 
       // PrintCurrInv();
    }

    public void MoveItem(InventoryItem item, int slot)  //To equip Slot. overload
    {

        if (item is Equippable converted_item)
        {
            try
            {
                CheckEquipSlot(converted_item, slot);
            }
            catch(ArgumentException e)
            {
                GD.PrintErr("Inventory.MoveItem: " + e.Message);
                return;
            }
            
            if (item.row != -1)
            {
                InventorySpace[item.row][item.column] = null;
            }
            else
            {
                EquippedSlotList[item.slot].ItemInSlot = null;
            }

            converted_item.SetPosition(slot);

            Events.Instance.EmitSignal(Events.SignalName.InventoryChanged); 
            //PrintCurrInv();
        }
        else
        {
            GD.PrintErr("Inventory: Unable to convert Item into Equippable. Can this item be equipped?");
        }
    }


    public void PrintCurrInv()
    {
        int i = 0;
        GD.PrintErr("-----------------------------------------------------------------------------------------------------------------");
        foreach (Godot.Collections.Array<InventoryItem> array  in InventorySpace)
        {
            foreach(InventoryItem item in array)
            {
                GD.PrintErr(i, ":", item, ",");
                i++;
            }
            GD.PrintErr("\n");
            i++;
        }
        GD.PrintErr("-----------------------------------------------------------------------------------------------------------------");
    }

    
    /*
    public void SetClothingSlot(Equippable item, int i) { //i for index
        if (item.EquipSlot != EquippedSlotList[i].WieldSlotType) {
            EquippedSlotList[i].ItemInSlot = item;
        }
    } */ //not focusing on this yet.
    
    //public List<ClothingSlot> WearableSlotList;\
}

