using UnityEngine;
using System.Collections;

public class EnemyMovementFlying : EnemyMovementBasic {

    protected override void DoMoveWithDirection(Vector2 dir)
    {
        CorrectLocalScale(dir.x);
        if (canMoveForward)
        {
            rBody.velocity = new Vector2(dir.x * speed, dir.y * speed);
            if (dir != Vector2.zero)
            {
                anim.SetBool("IsWalking", true);
            }
        }
        else
        {
            rBody.velocity = new Vector2(dir.x * speed, 0.5f);
            if (dir != Vector2.zero)
            {
                anim.SetBool("IsWalking", true);
            }
        }
    }
}
