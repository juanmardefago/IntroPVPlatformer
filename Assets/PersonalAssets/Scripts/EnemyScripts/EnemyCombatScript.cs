using UnityEngine;
using System.Collections;

public class EnemyCombatScript : MonoBehaviour {

    public int health;
    public float awarenessDistance;
    public float awarenessAngle;
    private Animator anim;
    public Transform playerTransform;
    private Vector2 playerPos2D;
    private Transform myTransform;
    private Vector2 myPos2D;
    private EnemyMovementBasic movementScript;
    private float aggro = 0f;

    private float dieDelay = 2f;
    private float dieTimer = 0f;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        movementScript = GetComponent<EnemyMovementBasic>();
	}
	
	// Update is called once per frame
	void Update () {
        playerPos2D = playerTransform.position;
        myPos2D = myTransform.position;

        if(dieTimer == 0f && (aggro > 0 || (Vector2.Distance(playerPos2D, myPos2D) < awarenessDistance && Vector2.Angle(myPos2D, playerPos2D) < awarenessAngle)))
        {
            movementScript.MoveTowardsPosition(playerPos2D);
            movementScript.MoveWithDirection((playerPos2D - myPos2D).normalized);
        }

        DecreaseAggro();
        CheckForDieDelay();
	}

    public void TakeDamage(int damage)
    {
        anim.SetTrigger("hit");
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
        aggro = 3f;
        movementScript.SetRecovering();
        movementScript.PushbackTo((myPos2D - playerPos2D).normalized);
    }

    private void Die()
    {
        dieTimer += Time.deltaTime;
        anim.SetTrigger("dying");
        //GetComponent<BoxCollider2D>().enabled = false;
    }

    private void DecreaseAggro()
    {
        aggro -= Time.deltaTime;
    }

    private void CheckForDieDelay()
    {
        if(dieTimer > 0f && dieTimer < dieDelay)
        {
            dieTimer += Time.deltaTime;
        } else if (dieTimer >= dieDelay)
        {
            Destroy(gameObject);
        }
    }
}
