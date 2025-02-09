using Godot;
using System;
using System.Data;
using System.Security.AccessControl;

[GlobalClass]
public partial class FurInvItem : InventoryItem
{
    public enum FurQuality
    {
        NOTSET,
        Perfect,
        Good,
        Decent,
        Poor,
        Shite
    }

    [Export]
    public FurQuality Quality = FurQuality.NOTSET;

    Godot.Collections.Dictionary<int, float> QualMultDict = new()
    {
        { (int)FurQuality.NOTSET, 0.0f },
        { (int)FurQuality.Perfect, 2.0f },
        { (int)FurQuality.Good, 1.5f },
        { (int)FurQuality.Decent, 1.0f },
        { (int)FurQuality.Poor, 0.6f },
        { (int)FurQuality.Shite, 0.2f }
    };

    public void SetValue()
    {
        if (QualMultDict.TryGetValue((int)Quality, out float mult))
        {
            Value = (int)(Value * mult);
        }
        else
        {
            GD.PrintErr($"Invalid fur quality {Quality} for setting fur {Name} value.");
        }
    }

}