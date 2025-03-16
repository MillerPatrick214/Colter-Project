using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

[GlobalClass]
public abstract partial class SelectableComponent : StaticBody3D
{
    bool isSelected;
    public Godot.Collections.Array<MeshInstance3D> MeshChildren {get; set;}
    bool MouseOn = false;

    public override void _Ready()
    {
        foreach (Node node in GetChildren())
        {
            if (node is MeshInstance3D mesh)
            {
                if (MeshChildren.Contains(mesh)) continue;
                MeshChildren.Add(mesh);
            }
        }
    }

    public void Selected()
    {

    }
    
    public void Highlight()
    {


    }
}



