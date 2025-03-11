using Godot;
using System;
using System.Collections.Generic;

public partial class ShaderManager  : Node
{
    Player player;
    public static ShaderManager Instance {get; private set;}
    public static Godot.Collections.Dictionary<Node3D, Vector3> VisibleEntityPositions;

    int count = 0;
    Image image;
    

    public override void _Ready()
    {
        if (Instance != this) return;
        Instance ??= this;
        player = Player.Instance;
        image = new();       
    }

    /*                                                        I'm saving this for me and jarred to go over. It's just too juicy
    public override void _Process(double delta)
    {
        int curr_count = VisibleEntityPositions.Count;

        RegisterOrUpdate(player, player.GlobalPosition);

        if (curr_count != count)
        {
            //

        }

        count = curr_count;
    }

    public void PackImage(ref Image image)
    {

    }
    public void RegisterOrUpdate(Node3D node_self, Vector3 global_position)
    {
        try
        {
            VisibleEntityPositions[node_self] = global_position;
        }
        catch (KeyNotFoundException)
        {
            VisibleEntityPositions.Add(node_self, global_position);
        }
    }

    public void OutOfSight(Node3D node_self)
    {
        try
        {
            VisibleEntityPositions.Remove(node_self);
        }
        catch (InvalidOperationException e)
        {
            GD.Print($"Error in ShaderManager: InvalidOperationException {e}");
        }
    }
    */



    

    



}
