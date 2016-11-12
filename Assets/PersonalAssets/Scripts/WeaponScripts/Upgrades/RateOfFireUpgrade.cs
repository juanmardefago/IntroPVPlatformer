using UnityEngine;
using System.Collections;
using System;

public class RateOfFireUpgrade : UpgradeScript {

    public override void ApplyEffect(Collider2D enemy, ProjectileScript projectile)
    {
        // Do nothing, not needed
    }

    public override void ApplyEffectToWeapon(WeaponScript weapon)
    {
        weapon.normalShotCD *= 0.5f;
    }

    public override void RemoveEffectFromWeapon(WeaponScript weapon)
    {
        weapon.normalShotCD *= 2;
    }

    public override void UpgradeProjectile(ProjectileScript projectile)
    {
        // Do nothing, not needed
    }
}
