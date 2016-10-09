using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour {

    public GameObject player;
    private PlayerCombatScript combatStats;
    private Text textUI;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        combatStats = player.GetComponent<PlayerCombatScript>();
        textUI = gameObject.GetComponent<Text>();
        textUI.text = "";
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
        textUI.text = currentHealth + " / " + maxHealth;
    }
}
