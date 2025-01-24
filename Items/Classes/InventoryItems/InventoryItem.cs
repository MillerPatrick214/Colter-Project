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
    public virtual int Value {get; set;} = 0;
    [Export]
    public virtual string ScenePath {get; set;} = "";
    
    public virtual List<List<int>> ItemSpace {get; set;} = new List<List<int>>
    {
        new List<int>{1},
    };
    
    public void ChangeQuantity(int quantity) {      
        if (IsStackable) {
            Quantity += quantity;
        }
    }
    
    public InventoryItem() : this("", "", null, 1, false, 0, new List<List<int>>{new List<int>{1}}){}

    public InventoryItem(string name, string description, ImageTexture texturePath, int quantity, bool isStackable, int value, List<List<int>> itemspace) {
        Name = name;
        Description = description;
        Texture = Texture;
        Quantity = quantity;
        IsStackable = isStackable;
        Value = value;
        ItemSpace = ItemSpace;

    } 
}
