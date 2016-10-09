using UnityEngine;
using System.Collections;

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

    public override void UpgradeProjectile(ProjectileScript projectile)
    {
        projectile.AddUpgrade(this);
    }
}
