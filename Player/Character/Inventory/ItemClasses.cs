using Godot;
using System.Collections.Generic;

[GlobalClass] 
public partial  class Item : Resource 
{
    public virtual string Name {get; set;} = "No Name";
    public virtual string Description {get; set;} = "No Description";
    public virtual string TexturePath {get; set;} = "";     //file path for now, eventually preload text I think
    public virtual int Quantity {get; set;} = 1;
    public virtual bool IsStackable {get; set;} = false;
    
    public virtual List<List<int>> ItemSpace {get; set;} = new List<List<int>>
    {
        new List<int>{1},
    };
}

[GlobalClass] 
public partial  class Wieldable : Item 
{
    public virtual string ScenePath {get; set;} = "";
    public virtual PackedScene Scene { get; set;} 
    public enum WieldSlot
    {
        NOTSET,
        Utility,
        PrimaryWeapon,
        SideArm,
        Melee 
    }
    public virtual WieldSlot EquipSlot {get; set;} = WieldSlot.NOTSET;

    public Wieldable(string ScenePath, WieldSlot EquipSlot) {
        this.ScenePath = ScenePath;
        this.EquipSlot = EquipSlot;
        Scene = GD.Load<PackedScene>(ScenePath);
    }
}

[GlobalClass] 
public partial class Wearable : Item 
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
public abstract partial class Consumable : Item 
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