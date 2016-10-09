using UnityEngine;
using System.Collections;
// Estoy es por ahora..
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerCombatScript : MonoBehaviour {

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

    private Inventory inventory;

    // Use this for initialization
    void Start () {
        inventory = GetComponent<Inventory>();
        anim = GetComponent<Animator>();
        RefreshLevelAndStats();
        expToLvlUp = ExperienceRequiredForNextLevel(level);
	}
	
	// Update is called once per frame
	void Update () {
        CheckForShotInput();
	}

    // Hay que checkear !EventSystem.current.IsPointerOverGameObject() para que si estoy en un button no se pueda disparar cuando clickeo los botones
    private void CheckForShotInput()
    {
        if (Input.GetButton("Fire1") && !EventSystem.current.IsPointerOverGameObject() && inventory.currentWeapon != null) {
            inventory.currentWeapon.Fire(OffsetForLocalScale());
        } else if (Input.GetButton("Fire2") && !EventSystem.current.IsPointerOverGameObject() && inventory.currentWeapon != null) {
            inventory.currentWeapon.ChargedFire(OffsetForLocalScale());
        } else if (Input.GetButton("Reload") && inventory.currentWeapon != null)
        {
            inventory.currentWeapon.ReloadIfNeeded();
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

    public void ReceiveHeal(int heal)
    {
        if(health + heal <= maxHealth)
        {
            health += heal;
        } else
        {
            health = maxHealth;
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
