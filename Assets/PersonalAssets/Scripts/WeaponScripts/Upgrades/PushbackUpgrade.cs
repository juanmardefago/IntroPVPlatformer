using UnityEngine;
using System.Collections;

public class PushbackUpgrade : UpgradeScript
{
    public override void ApplyEffect(Collider2D enemy, ProjectileScript projectile)
    {
        enemy.gameObject.SendMessage("Pushback");
    }

    public override void UpgradeProjectile(ProjectileScript projectile)
    {
        projectile.AddUpgrade(this);
    }
}
