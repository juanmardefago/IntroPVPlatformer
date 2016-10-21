using UnityEngine;
using System.Collections;

public class EnemyCombatScript : MonoBehaviour
{

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

    public int damage;

    public int expBounty;

    private PopupTextHandler popup;

    // Use this for initialization
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        movementScript = GetComponent<EnemyMovementBasic>();
        popup = GetComponent<PopupTextHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPos2D = playerTransform.position;
        myPos2D = myTransform.position;

        if (dieTimer == 0f && (aggro > 0 || (Vector2.Distance(playerPos2D, myPos2D) < awarenessDistance && Vector2.Angle(myPos2D, playerPos2D) < awarenessAngle)))
        {
            movementScript.MoveTowardsPosition(playerPos2D);
            movementScript.MoveWithDirection(DirectionPointingToPlayer());
        }

        DecreaseAggro();
        CheckForDieDelay();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player" && dieTimer == 0f)
        {
            collision.transform.gameObject.SendMessage("TakeDamage", damage);
            collision.transform.gameObject.SendMessage("Pushback", DirectionPointingToPlayer());
        }
    }

    public void TakeDamage(int damage)
    {
        anim.SetTrigger("hit");
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        popup.Show(damage.ToString());
        aggro = 3f;
    }

    public void Pushback()
    {
        movementScript.SetRecovering();
        movementScript.PushbackTo(-DirectionPointingToPlayer());
    }

    public void Die()
    {
        playerTransform.SendMessage("AddExperience", expBounty);
        dieTimer += Time.deltaTime;
        anim.SetTrigger("dying");
        movementScript.MakeKinematic();
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void DecreaseAggro()
    {
        aggro -= Time.deltaTime;
    }

    private void CheckForDieDelay()
    {
        if (dieTimer > 0f && dieTimer < dieDelay)
        {
            dieTimer += Time.deltaTime;
        }
        else if (dieTimer >= dieDelay)
        {
            Destroy(gameObject);
        }
    }

    private Vector2 DirectionPointingToPlayer()
    {
        Vector2 res;
        if (playerPos2D.x > myPos2D.x)
        {
            res = Vector2.right;
        }
        else if (myPos2D.x > playerPos2D.x)
        {
            res = Vector2.left;
        } else
        {
            res = Vector2.zero;
        }
        return res;
    }
}
