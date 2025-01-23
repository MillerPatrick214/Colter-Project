using Godot;
using System;

[GlobalClass] 
public partial class Wearable : InventoryItem 
{
        public enum WearSlot
    {
        NOTSET,
        Head,
        Chest,
        Legs,
        Back,               //Could also be insulating/fur coat layer?
    }
    public virtual WearSlot EquipSlot {get; set;} = WearSlot.NOTSET;
}

[GlobalClass] 
public abstract partial class Consumable : InventoryItem 
{
    [Signal]
    public delegate void ConsumedEventHandler(Consumable this_consumable);    // Eventually, put this in Events Singleton not here. 
    public abstract void Consume(); //In this case it would EmitSignal(SignalName.Consumed, this) however I want to run this thru Events.
    ConsumableEffect Effect = null;
}

[GlobalClass] 
public partial class ConsumableEffect : Resource 
{
    //Effect should signal 
}