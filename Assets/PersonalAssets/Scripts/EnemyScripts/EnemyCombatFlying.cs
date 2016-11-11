using UnityEngine;
using System.Collections;

public class EnemyCombatFlying : EnemyCombatScript {

    // Update is called once per frame
    public new void Update()
    {
        playerPos2D = playerTransform.position;
        myPos2D = myTransform.position;

        if (dieTimer == 0f && (aggro > 0 || Physics2D.OverlapCircle(myPos2D, awarenessDistance, playerLayer)))
        {
            movementScript.MoveWithDirection(DirectionPointingToPlayer());
        }
        else
        {
            movementScript.MoveWithDirection(Vector2.zero);
        }

        DecreaseAggro();
        CheckForDieDelay();
    }

    protected new Vector2 DirectionPointingToPlayer()
    {
        Vector2 res = base.DirectionPointingToPlayer();
        if (playerPos2D.y > myPos2D.y)
        {
            res += Vector2.up;
        }
        else if (myPos2D.y > playerPos2D.y)
        {
            res += Vector2.down;
        }
        else
        {
            res = Vector2.zero;
        }
        return res;
    }

}
