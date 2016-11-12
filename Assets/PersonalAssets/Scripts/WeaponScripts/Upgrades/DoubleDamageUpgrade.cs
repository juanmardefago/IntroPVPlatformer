using UnityEngine;
using System.Collections;
using System;

public class DoubleDamageUpgrade : UpgradeScript
{

    public override void ApplyEffect(Collider2D enemy, ProjectileScript projectile)
    {
        // Do nothing, not needed;
    }

    public override void ApplyEffectToWeapon(WeaponScript weapon)
    {
        // Do nothing, not needed;
    }

    public override void RemoveEffectFromWeapon(WeaponScript weapon)
    {
        // Do nothing, not needed;
    }

    public override void UpgradeProjectile(ProjectileScript projectile)
    {
        projectile.damage = projectile.damage * 2;
    }
}
