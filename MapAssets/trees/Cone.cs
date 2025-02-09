using Godot;
using System;

public partial class Cone : MeshInstance3D
{
    [Export]
    public float BendStrength = 0.3f;
    [Export]
    public float WindStrength = 0.3f;
    [Export]
    public Vector3 WindDirection = new Vector3(1, 0, 0);

    private Material _material;
    private float _topY;

    public override void _Ready()
    {
        CalculateTopY();

        // Calculate the topmost Y value of the tree
        // Set the top Y value as a uniform for the shader
        Set("shader_param/_TopY", _topY);
        Set("shader_param/bend_strength", BendStrength);
        Set("shader_param/wind_strength", WindStrength);
        Set("shader_param/wind_direction", WindDirection);
	}

    private void CalculateTopY()
    {
        Mesh mesh = GetMesh();

        // Calculate the maximum Y value from the tree's mesh vertices
        _topY = float.MinValue;
        foreach (Vector3 vertex in mesh.GetFaces())
        {
            _topY = Mathf.Max(_topY, vertex.Y);
        }

		//GD.PrintErr(_topY);
    }
}

