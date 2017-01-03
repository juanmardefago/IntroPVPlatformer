using UnityEngine;
using System.Collections;

public class EnemyCombatScript : MonoBehaviour, Damagable
{
    public string enemyName;
    public int level;
    public int baseHealth;
    private int health;
    private int maxHealth;
    public float awarenessDistance;
    public LayerMask playerLayer;
    protected Animator anim;
    public Transform playerTransform;
    protected Vector2 playerPos2D;
    protected Transform myTransform;
    protected Vector2 myPos2D;
    protected EnemyMovementBasic movementScript;
    protected float aggro = 0f;

    protected float dieDelay = 2f;
    protected float dieTimer = 0f;

    public int baseDamage;
    [HideInInspector]
    public int damage;
    [HideInInspector]
    public int deviation;
    public float hitRate;
    public float critRate;
    [HideInInspector]
    public bool crit;

    public int baseExpBounty;
    private int expBounty;
    public bool waterType;

    public float hitCD;
    private bool hitting;

    protected PopupTextHandler popup;
    protected SoundScript soundScript;

    // Use this for initialization
    public void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        movementScript = GetComponent<EnemyMovementBasic>();
        soundScript = GetComponent<SoundScript>();
        popup = GetComponent<PopupTextHandler>();
        deviation = Mathf.Max((damage / 100) * 5, 1);
        AdjustStatsForLevel();
    }

    // Update is called once per frame
    public void Update()
    {
        playerPos2D = playerTransform.position;
        myPos2D = myTransform.position;

        if (dieTimer == 0f && (aggro > 0 || Physics2D.OverlapCircle(myPos2D, awarenessDistance, playerLayer)) && !SameXAsPlayer())
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" && dieTimer == 0f)
        {
            if (!hitting) StartCoroutine(HitPlayer(collision.transform.gameObject));
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" && dieTimer == 0f)
        {
            if (!hitting) StartCoroutine(HitPlayer(collision.transform.gameObject));
        }
    }

    private IEnumerator HitPlayer(GameObject player)
    {
        hitting = true;
        if (Random.value <= hitRate)
        {
            if (Random.value <= critRate)
            {
                crit = true;
            }
            player.SendMessage("ProcessHit", this);
            soundScript.PlayAttackSound();
        }
        else
        {
            player.SendMessage("ShowMiss");
        }

        yield return new WaitForSeconds(hitCD);
        hitting = false;
    }

    // para que no rompa con la version vieja.
    public void TakeDamage(int damage)
    {
        DoTakeDamage(damage);
    }

    public void TakeDamage(int damage, Color? color = null)
    {
        DoTakeDamage(damage, color);
    }

    public void TakeCritDamage(int damage)
    {

        DoTakeDamage(damage, Color.red);
    }

    public void DoTakeDamage(int damage, Color? color = null)
    {
        if (color != null)
        {
            Show(damage.ToString(), (Color)color);
        }
        else
        {
            Show(damage.ToString());
        }
        aggro = 3f;
        soundScript.PlayHitSound();
        anim.SetTrigger("hit");
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Show(string text)
    {
        popup.Show(text);
    }

    private void Show(string text, Color color)
    {
        popup.Show(text, color);
    }

    public void ShowMiss()
    {
        Show("Miss", Color.grey);
    }

    public void Pushback()
    {
        movementScript.SetRecovering();
        movementScript.PushbackTo(new Vector2(-DirectionPointingToPlayer().x, 0), 3);
    }

    public void PushbackShield()
    {
        movementScript.SetRecovering();
        movementScript.PushbackTo(new Vector2(-DirectionPointingToPlayer().x, 0), 20);
    }

    public void Die()
    {
        GetComponent<BuffSystem>().ClearBuffs();
        playerTransform.SendMessage("AddExperience", expBounty);
        dieTimer += Time.deltaTime;
        anim.SetTrigger("dying");
        movementScript.MakeKinematic();
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<LootDropScript>().DropLoot();
        soundScript.PlayDeathSound();
        GetComponent<EnemyHealthBarHandler>().Disable();
        GetComponent<EnemyNameHandler>().Disable();
    }

    public void NoExpDie()
    {
        GetComponent<BuffSystem>().ClearBuffs();
        dieTimer += Time.deltaTime;
        anim.SetTrigger("dying");
        movementScript.MakeKinematic();
        GetComponent<BoxCollider2D>().enabled = false;
        soundScript.PlayDeathSound();
        GetComponent<EnemyHealthBarHandler>().Disable();
        GetComponent<EnemyNameHandler>().Disable();
    }

    protected void DecreaseAggro()
    {
        if (aggro > 0)
        {
            aggro -= Time.deltaTime;
        }
        else if (aggro < 0)
        {
            aggro = 0;
        }
    }

    protected void CheckForDieDelay()
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

    public virtual Vector2 DirectionPointingToPlayer()
    {
        Vector2 res;
        if (playerPos2D.x > myPos2D.x)
        {
            res = Vector2.right;
        }
        else if (myPos2D.x > playerPos2D.x)
        {
            res = Vector2.left;
        }
        else
        {
            res = Vector2.zero;
        }
        return res;
    }

    protected bool SameXAsPlayer()
    {
        return myPos2D.x >= playerPos2D.x - 0.1 && myPos2D.x <= playerPos2D.x + 0.1;
    }

    public void OnWaterTouch()
    {
        if (!waterType)
        {
            if (health < baseHealth)
            {
                Die();
            }
            else
            {
                NoExpDie();
            }
        }
    }

    public void OnWaterLack()
    {
        if (waterType)
        {
            if (health < baseHealth)
            {
                Die();
            }
            else
            {
                NoExpDie();
            }
        }
    }

    public void AdjustStatsForLevel()
    {
        health = baseHealth + ((baseHealth / 2) * level);
        maxHealth = health;
        damage = baseDamage + ((baseDamage / 3) * level);
        expBounty = baseExpBounty + ((baseExpBounty / 2) * level);
    }

    public float HealthPercentage()
    {
        return (float) health / maxHealth;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, awarenessDistance);
    }
}
