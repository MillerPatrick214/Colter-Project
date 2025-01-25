using Godot;
using System;

[GlobalClass] 
public abstract partial class Consumable : InventoryItem 
{
    [Signal]
    public delegate void ConsumedEventHandler(Consumable this_consumable);    // Eventually, put this in Events Singleton not here. 
    public abstract void Consume(); //In this case it would EmitSignal(SignalName.Consumed, this) however I want to run this thru Events.
    ConsumableEffect Effect = null;
}
