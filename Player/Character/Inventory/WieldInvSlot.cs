using Godot;
using System;

public partial class WieldInvSlot : Resource 
{   
    public Wieldable ItemInSlot {get; set;} = null;
    public Wieldable.WieldSlot WieldSlotType {get; set;} = Wieldable.WieldSlot.NOTSET;
    public WieldInvSlot(Wieldable ItemInSlot, Wieldable.WieldSlot WieldSlotType) {
        this.ItemInSlot = ItemInSlot;
        this.WieldSlotType = WieldSlotType;
    }

    public WieldInvSlot() : this(null, 0){} 
}