using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class InventoryItem : Resource 
{
    [Export]
    public virtual string Name {get; set;} = "No Name";
    [Export]
    public virtual string Description {get; set;} = "No Description";
    [Export]
    public virtual Texture2D Texture {get; set;} = GD.Load<CompressedTexture2D>("res://NPC/Animal/TestCapy/CapyBoy_capybara2_texture.png");
    [Export]
    public virtual int Quantity {get; set;} = 1;
    [Export]
    public virtual bool IsStackable {get; set;} = false;
    [Export]
    public virtual int CurrentStack {get; set;} = 1;
    [Export]
    public virtual int MaxStack {get; set;} = 1;
    [Export]
    public virtual int Value {get; set;} = 0;
    [Export]
    public virtual string ScenePath {get; set;} = null;
    [Export]
    public virtual int row {get; set;} = -1;
    [Export]
    public virtual int column {get; set;} = -1;
    [Export]
    public virtual int slot {get; set;} = -1;

    
    public virtual List<List<int>> ItemSpace {get; set;} = new List<List<int>>
    {
        new List<int>{1},
    };
    
    public void ChangeQuantity(int quantity) {      
        if (IsStackable) {
            Quantity += quantity;
        }
    }
    
    public InventoryItem() : this("", "", null, 1, false, 1, 1, 0, new List<List<int>>{new List<int>{1}}){}

    public InventoryItem(string name, string description, ImageTexture texture, int quantity, bool isStackable, int currentstack, int maxstack,  int value, List<List<int>> itemspace) {
        Name = name;
        Description = description;
        Texture = texture;
        Quantity = quantity;
        IsStackable = isStackable;
        CurrentStack = currentstack;
        MaxStack = maxstack;
        Value = value;
        ItemSpace = itemspace;

    }

    public void SetPosition(int row, int column)
    {
        this.row = row;
        this.column = column;
        slot = -1;
    }

    public void SetPosition(int slot_index)
    {
        slot = slot_index;
        row = -1;
        column = -1;

    }

    public void SetStack(int i) //probably want an add sub stack w/ error handling etc.
    {
        CurrentStack = i;
    }

    public void SetMaxStack(int i)
    {
        MaxStack = i;
    }
}
