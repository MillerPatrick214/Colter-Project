using Godot;
using System;

public partial class AnimalHurtBox : Area3D
{
    [Export] public float StrikeDamage;

    public override void _Ready()
    {
        AreaEntered += (area) => Attack(area);
    }
    public void SetDamage(float damage)
    {
        StrikeDamage = damage;
    }
    public void Attack(Area3D area)
    {
        if (area is not HitBoxComponent hitbox) return;
        if (StrikeDamage == 0) GD.PrintErr($"{GetPath()} Error in hurtbox. Attempted to attack but no strike damage set");
        hitbox.Damage(StrikeDamage);
    }
}
