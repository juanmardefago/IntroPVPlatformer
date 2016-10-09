using UnityEngine;
using System.Collections;

public class DoubleDamageUpgrade : UpgradeScript
{
    public override void ApplyEffect(Collider2D enemy, ProjectileScript projectile)
    {
        // Do nothing, not needed;
    }

    public override void UpgradeProjectile(ProjectileScript projectile)
    {
        projectile.damage = projectile.damage * 2;
    }
}
