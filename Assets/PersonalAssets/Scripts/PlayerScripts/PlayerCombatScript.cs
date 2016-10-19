﻿using UnityEngine;
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
    public int Level { get { return level; } }
    private int expToLvlUp;
    public int ExperienceToNextLevel { get { return expToLvlUp; } }

    private PopupTextHandler popup;

    private Inventory inventory;

    public GameObject shield;
    private bool blocking = false;
    private bool canBlock = true;

    // Use this for initialization
    void Start () {
        inventory = GetComponent<Inventory>();
        popup = GetComponent<PopupTextHandler>();
        anim = GetComponent<Animator>();
        RefreshLevelAndStats();
        expToLvlUp = ExperienceRequiredForNextLevel();
	}
	
	// Update is called once per frame
	void Update () {
        CheckForShotInput();
        // Probando hacer el Cooldown del block usando yield y coroutines
        if (!blocking) StartCoroutine(CheckForBlock());
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

    private IEnumerator CheckForBlock()
    {
        if (Input.GetButtonDown("Shield") && canBlock)
        {
            Shield();
            yield return new WaitForSeconds(1f);
            Unshield();
            yield return new WaitForSeconds(1f);
            canBlock = true;
        } 
    }

    // Acá se le va a avisar al animator del shield que haga la animación de aparecion del escudo
    private void Shield()
    {
        blocking = true;
        shield.SetActive(true);
        canBlock = false;
    }

    // Acá se le va a avisar al animator del shield que haga la animación de desaparición del escudo
    private void Unshield()
    {
        blocking = false;
        shield.SetActive(false);
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
        int damageTaken = damage;
        if (!blocking)
        {
            health -= damageTaken;
            anim.SetTrigger("hit");
        } else
        {
            damageTaken = damage / 5;
            health -= damageTaken;
        }
        if (health <= 0)
        {
            Die();
        }
        popup.Show(damageTaken.ToString());
    }

    public void ReceiveHeal(int heal)
    {
        if(health + heal <= maxHealth)
        {
            health += heal;
            popup.Show(heal.ToString(), Color.green);
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
            popup.Show("Level up!", Color.yellow);
            RefreshLevelAndStats();
            AddExperience(exp);
        } else
        {
            experience += exp;
        }
    }

    private void RefreshLevelAndStats()
    {
        maxHealth = baseHealth + (level * level * healthPerLevel) + bonusHealth;
        health = maxHealth;
        expToLvlUp = ExperienceRequiredForNextLevel();
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else {
            lives--;
            transform.position = new Vector3(-6f, -1.35f, 0f);
            health = maxHealth;
        }

    }
}
