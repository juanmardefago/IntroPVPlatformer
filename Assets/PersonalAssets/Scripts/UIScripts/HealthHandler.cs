using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour {

    public GameObject player;
    private PlayerCombatScript combatStats;
    private Image healthBar;
    private Text textUI;

    // Use this for initialization
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        combatStats = player.GetComponent<PlayerCombatScript>();
        healthBar = gameObject.GetComponent<Image>();
        healthBar.fillAmount = 1;
        textUI = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleHealthAmount();
    }

    private void HandleHealthAmount()
    {
        int currentHealth = combatStats.Health;
        int maxHealth = combatStats.MaxHealth;
        healthBar.fillAmount = (float) currentHealth / maxHealth;
        textUI.text = currentHealth + " / " + maxHealth;
    }
}
