﻿using UnityEngine;
using System.Collections;
using System;

public class PushbackUpgrade : UpgradeScript
{
    public override void ApplyEffect(Collider2D enemy, ProjectileScript projectile)
    {
        enemy.gameObject.SendMessage("Pushback");
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
        projectile.AddUpgrade(this);
    }
}
