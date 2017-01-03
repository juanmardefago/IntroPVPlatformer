using UnityEngine;
using System.Collections;


public class SlowDebuff : BuffScript
{

    private float oldSpeed;

    public override void ApplyBaseEffect(GameObject unit)
    {
        if(unit.tag == "Player")
        {
            PlayerMovement movementScript = unit.GetComponent<PlayerMovement>();
            movementScript.baseMovementSpeed = movementScript.baseMovementSpeed * 0.5f;
            movementScript.speed = movementScript.baseMovementSpeed;
        } else
        {
            EnemyMovementBasic movementScript = unit.GetComponent<EnemyMovementBasic>();
            oldSpeed = movementScript.speed;
            movementScript.speed = oldSpeed * 0.5f;
        }
    }

    public override void RemoveBaseEffect(GameObject unit)
    {
        if (unit.tag == "Player")
        {
            PlayerMovement movementScript = unit.GetComponent<PlayerMovement>();
            movementScript.baseMovementSpeed = movementScript.originalMovementSpeed;
            movementScript.speed = movementScript.originalMovementSpeed;
        }
        else
        {
            EnemyMovementBasic movementScript = unit.GetComponent<EnemyMovementBasic>();
            movementScript.speed = oldSpeed;
        }
    }
}
