using UnityEngine;
using System.Collections;
using System;

public class VampiricUpgrade : UpgradeScript
{
    private GameObject player;

    public override void ApplyEffect(Collider2D enemy, ProjectileScript projectile)
    {
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        int heal = (projectile.damage / 4);
        player.SendMessage("ReceiveHeal", heal);
    }

    public override void ApplyEffectToWeapon(WeaponScript weapon)
    {
        // Do nothing, not needed
    }

    public override void RemoveEffectFromWeapon(WeaponScript weapon)
    {
        // Do nothing, not needed
    }

    public override void UpgradeProjectile(ProjectileScript projectile)
    {
        projectile.AddUpgrade(this);
    }
}
