using Godot;
using System;
using System.Threading.Tasks;

[Tool]
public partial class RodSelectableBody : StaticBody3D
{
    [Signal] public delegate void BarrelLoadedEventHandler();
    [Signal] public delegate void RodStowedEventHandler(); 
    [ExportToolButton("Print Current Transform (in C# for new Tranform3D)")] public Callable PrintTransformButton => Callable.From(PrintTransform);

    AnimationPlayer AniPlayer;
    /*
    [ExportToolButton("Play To Barrel Anim")] public Callable SetToBarrelButton => Callable.From(SetToBarrel);
    [ExportToolButton("Play To Stowed Anim)")] public Callable SetToStowedButton => Callable.From(SetToStowed);
    */
    
    public RodState curr_rod_state = RodState.STOWED;

    bool mouse_on = false;

    bool animation_finished = true;
    
    public enum RodState 
    {
        STOWED,
        IN_BARREL
    }



    /*
    public void SetToBarrel()
    {
        Transform = InBarrelTip;
    }
    
    public void SetToStart()
    {
        Transform = StartPosition;
    }
    */

    public override void _Ready()
    {
        AniPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
        MouseEntered += () => mouse_on = true;
        MouseExited += () => mouse_on = false;
    }

    public void PrintTransform()
    {
        GD.PrintRich($"[color=green]new Transform3D({Transform}); [/color]");
    }

    public override async void _Process(double delta)
    {
        
        if (curr_rod_state == RodState.STOWED)
        {
            if (mouse_on && Input.IsActionJustPressed("UseItem"))
            {
                AniPlayer.Play("RodIntoBarrel");
                await ToSignal(AniPlayer, AnimationPlayer.SignalName.AnimationFinished);
                EmitSignal(SignalName.BarrelLoaded);
                curr_rod_state = RodState.IN_BARREL;
            }
        }

        if (curr_rod_state == RodState.IN_BARREL)
        {
            if (mouse_on && Input.IsActionJustPressed("UseItem"))
            {
                AniPlayer.Play("StowRod");
                await ToSignal(AniPlayer, AnimationPlayer.SignalName.AnimationFinished);
                EmitSignal(SignalName.RodStowed);
                curr_rod_state = RodState.STOWED;
            }
        }
    }
}




