using Godot;
using System;


public partial class ClothingSlot : Resource          
{
    [Export]
    public Wearable EquipedWearable{get; set;}
    [Export]
    public Wearable.WearSlot WearSlotType{get; set;} = Wearable.WearSlot.NOTSET;
    public ClothingSlot(Wearable EquipedWearable, Wearable.WearSlot WearSlotType) {
        this.EquipedWearable = EquipedWearable;
        this.WearSlotType = WearSlotType;
    }
    public ClothingSlot() {}
}
