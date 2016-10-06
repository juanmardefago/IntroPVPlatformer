using UnityEngine;
using System.Collections;
// Estoy es por ahora..
using UnityEngine.SceneManagement;

public class PlayerCombatScript : MonoBehaviour {

    private WeaponScript weapon;
    private int health;
    private int maxHealth;
    public int baseHealth;
    public int healthPerLevel;
    private int bonusHealth;
    public int Health { get { return health; } }
    public int MaxHealth { get { return maxHealth; } }

    public int lives;

    private Animator anim;
    [SerializeField]
    private int experience;
    public int Experience { get { return experience; } }
    [SerializeField]
    private int level;
    private int expToLvlUp;
    public int ExperienceToNextLevel { get { return expToLvlUp; } }

    // Use this for initialization
    void Start () {
        weapon = GetComponentInChildren<WeaponScript>();
        anim = GetComponent<Animator>();
        RefreshLevelAndStats();
        expToLvlUp = ExperienceRequiredForNextLevel(level);
	}
	
	// Update is called once per frame
	void Update () {
        CheckForShotInput();
	}

    private void CheckForShotInput()
    {
        if (Input.GetButtonDown("Fire1")) {
            weapon.Fire(OffsetForLocalScale());
        } else if (Input.GetButtonDown("Fire2")) {
            weapon.ChargedFire(OffsetForLocalScale());
        }
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

    public void TakeDamage(int damage)
    {
        health -= damage;
        anim.SetTrigger("hit");
        if(health == 0)
        {
            Die();
        }
    }

    public void AddExperience(int exp)
    {
        if (exp >= expToLvlUp)
        {
            exp -= expToLvlUp;
            experience += expToLvlUp;
            level++;
            RefreshLevelAndStats();
            AddExperience(exp);
        } else
        {
            experience += exp;
        }
    }

    private void RefreshLevelAndStats()
    {
        maxHealth = baseHealth + (level * healthPerLevel) + bonusHealth;
        health = maxHealth;
    }
    
    private int ExperienceRequiredForNextLevel(int currentLevel)
    {
        return 1000 + (currentLevel * (currentLevel - 1) * 100);
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else {
            lives--;
            transform.position = new Vector3(-6f, -1.35f, 0f);
            health = maxHealth;
        }

    }
}
