using UnityEngine;
using System.Collections;
// Estoy es por ahora..
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerCombatScript : MonoBehaviour, Damagable {

    private int health;
    private int maxHealth;
    public int baseHealth;
    public int healthPerLevel;
    private int bonusHealth;
    public int Health { get { return health; } }
    public int MaxHealth { get { return maxHealth; } }

    public int lives;
    public Transform respawnTransform;

    private Animator anim;
    [SerializeField]
    private int experience;
    public int Experience { get { return experience; } }
    [SerializeField]
    private int level;
    public int Level { get { return level; } }
    private int expToLvlUp;
    private int totalExpForNextLevel;
    public int ExperienceToNextLevel { get { return totalExpForNextLevel; } }

    private PopupTextHandler popup;

    private Inventory inventory;
    private PlayerMovement movementScript;

    public GameObject shield;
    public float shieldCD;
    private float shieldTimerCD;
    public float shieldDuration;
    private bool blocking = false;
    private bool canBlock = true;

    private SoundScript soundScript;

    public float shootingSlowCD;
    private float shootingSlowTimer;

    // Use this for initialization
    void Start () {
        inventory = GetComponent<Inventory>();
        popup = GetComponent<PopupTextHandler>();
        anim = GetComponent<Animator>();
        soundScript = GetComponent<SoundScript>();
        movementScript = GetComponent<PlayerMovement>();
        RefreshLevelAndStats();
        totalExpForNextLevel = ExperienceRequiredForNextLevel();
        expToLvlUp = totalExpForNextLevel;
	}
	
	// Update is called once per frame
	void Update () {
        CheckForShotInput();
        CheckAndDecreaseSlowTimer();
        // Probando hacer el Cooldown del block usando yield y coroutines
        if (canBlock && Input.GetButtonDown("Shield")) StartCoroutine(Block());
        if (shieldTimerCD != 0f) DecreaseShieldTimer();
	}

    private void CheckAndDecreaseSlowTimer()
    {
        if (shootingSlowTimer >= Time.deltaTime)
        {
            shootingSlowTimer -= Time.deltaTime;
        }
        else if (shootingSlowTimer < Time.deltaTime)
        {
            shootingSlowTimer = 0f;
            movementScript.speed = movementScript.baseMovementSpeed;
        }
    }

    private void DecreaseShieldTimer()
    {
        if(shieldTimerCD > Time.deltaTime)
        {
            shieldTimerCD -= Time.deltaTime;
        } else
        {
            shieldTimerCD = 0f;
        }
    }

    public float ShieldCDFill()
    {
        return (shieldCD - shieldTimerCD)/shieldCD;
    }

    public float ChargedShotCDFill()
    {
        return inventory.currentWeapon.ChargedShotCDFill();
    }

    // Hay que checkear !EventSystem.current.IsPointerOverGameObject() para que si estoy en un button no se pueda disparar cuando clickeo los botones
    private void CheckForShotInput()
    {
        if (Input.GetButton("Fire1") && !EventSystem.current.IsPointerOverGameObject() && inventory.currentWeapon != null) {
            inventory.currentWeapon.Fire(OffsetForLocalScale());
            if (!inventory.currentWeapon.Reloading())
            {
                movementScript.speed = movementScript.baseMovementSpeed * 0.5f;
                shootingSlowTimer = shootingSlowCD;
            }
        } else if (Input.GetButton("Fire2") && !EventSystem.current.IsPointerOverGameObject() && inventory.currentWeapon != null) {
            inventory.currentWeapon.ChargedFire(OffsetForLocalScale());
        } else if (Input.GetButton("Reload") && inventory.currentWeapon != null)
        {
            inventory.currentWeapon.ReloadIfNeeded();
        }
    }

    private IEnumerator Block()
    {
        Shield();
        yield return new WaitForSeconds(shieldDuration);
        Unshield();
        shieldTimerCD = shieldCD;
        yield return new WaitForSeconds(shieldCD);
        shield.SetActive(false);
        canBlock = true;
    }

    // Acá se le va a avisar al animator del shield que haga la animación de aparecion del escudo
    private void Shield()
    {
        blocking = true;
        shield.SetActive(true);
        shield.GetComponent<Animator>().SetTrigger("in");
        canBlock = false;
    }

    // Acá se le va a avisar al animator del shield que haga la animación de desaparición del escudo
    private void Unshield()
    {
        blocking = false;
        shield.GetComponent<Animator>().SetTrigger("out");
    }


    private int OffsetForLocalScale()
    {
        int res = 0;
        if (transform.localScale.x > 0) {
            res = 1;
        } else {
            res = -1;
        }
        return res;
    }

    public void ProcessHit(EnemyCombatScript enemy)
    {
        int enemyDamage = NormalDistribution.CalculateNormalDistRandom(enemy.damage, enemy.deviation);
        if (enemy.crit)
        {
            TakeDamage(enemyDamage * 2, Color.red);
            enemy.crit = false;
        } else
        {
            TakeDamage(enemyDamage);
        }

        if (!blocking)
        {
            movementScript.Pushback(enemy.DirectionPointingToPlayer());
        } else
        {
            enemy.PushbackShield();
        }
    }

    public void TakeDamage(int damage, Color? color = null)
    {
        int damageTaken = damage;
        if (!blocking)
        {
            anim.SetTrigger("hit");
        } else
        {
            damageTaken = damage / 5;
        }
        DoTakeDamage(damageTaken, color);
    }

    public void TakeSpikeDamage(int damage)
    {
        int damageTaken = damage;
        if (!blocking)
        {
            anim.SetTrigger("hit");
        }
        else
        {
            damageTaken = damage / 5;
        }
        DoTakeDamage(damageTaken, Color.white);
    }

    private void DoTakeDamage(int damage, Color? color = null)
    {
        soundScript.PlayHitSound();
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        if (color != null)
        {
            Show(damage.ToString(), (Color) color);
        } else
        {
            Show(damage.ToString());
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

    public void ReceiveHeal(int heal)
    {
        if(health + heal <= maxHealth)
        {
            health += heal;
            Show(heal.ToString(), Color.green);
        } else
        {
            health = maxHealth;
        }
    }

    public void AddExperience(int exp)
    {
        int expGained = exp;
        if (expGained >= expToLvlUp)
        {
            expGained -= expToLvlUp;
            level++;
            Show("Level up!", Color.yellow);
            RefreshLevelAndStats();
            AddExperience(expGained);
        } else
        {
            experience += expGained;
            expToLvlUp -= expGained;
        }
    }

    private void RefreshLevelAndStats()
    {
        maxHealth = baseHealth + (level  * healthPerLevel) + bonusHealth;
        health = maxHealth;
        totalExpForNextLevel = ExperienceRequiredForNextLevel();
        expToLvlUp = totalExpForNextLevel;
        experience = 0;
    }
    
    private int ExperienceRequiredForNextLevel()
    {
        return 1000 + (level * (level - 1) * 100);
    }

    public void HealthUpgrade(int upgrade)
    {
        bonusHealth += upgrade;
    }

    // Esto es para que haya una implementacion de la muerte (?
    // No es la version final ni a palos jajaja
    public void Die()
    {
        if(lives == 0)
        {
            SceneManager.LoadScene("YouDiedDemo");
        } else {
            lives--;
            if (respawnTransform != null)
            {
                transform.position = respawnTransform.position;
            } else
            {
                transform.position = Vector3.zero;
            }
            health = maxHealth;
        }
        GetComponent<BuffSystem>().ClearBuffs();
        soundScript.PlayDeathSound();
    }
}
