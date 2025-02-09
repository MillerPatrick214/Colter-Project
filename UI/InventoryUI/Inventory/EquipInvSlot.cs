using Godot;
using System;

public partial class EquipInvSlot : Resource 
{   
    [Export]
    public Equippable ItemInSlot {get; set;} = null;
    [Export]
    public Equippable.EquipSlot EquipSlotType {get; set;} = Equippable.EquipSlot.NOTSET;

    public EquipInvSlot(Equippable ItemInSlot, Equippable.EquipSlot EquipSlotType) {
        this.ItemInSlot = ItemInSlot;
        this.EquipSlotType = EquipSlotType;
    }

    public EquipInvSlot() : this(null, Equippable.EquipSlot.NOTSET){} 
}