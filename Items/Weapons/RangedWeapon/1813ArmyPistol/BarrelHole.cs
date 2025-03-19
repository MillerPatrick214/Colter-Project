using Godot;
using System;
using System.Threading.Tasks;

public partial class BarrelHole : Area3D
{
    
    SceneTreeTimer timer;

    
    public BallSelectableBody InTip = null;

    public override void _Ready()
    {
        BodyEntered += (body) => PutInTip(body);
        BodyExited += (body) => RemoveFromTip(body);
    }

    public void PutInTip(Node3D body)
    {
        GD.PrintErr("BarrelHole: BodyEntered");
        if (body is BallSelectableBody ball && InTip == null)
        {
            InTip = ball;
            ball.SnapToBarrelTip(GlobalPosition);
            GD.PrintErr("BarrelHole: Ball put into tip!!");
        }
    }

    public void RemoveFromTip(Node3D body)
    {
        if (body is BallSelectableBody ball)
        {
            if (InTip != ball) return;
            InTip = null;
        }
    }

}
