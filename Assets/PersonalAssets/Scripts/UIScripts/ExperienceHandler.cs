using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExperienceHandler : MonoBehaviour {

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
        HandleExperience();
    }

    private void HandleExperience()
    {
        int currentXP = combatStats.Experience;
        int nextLvlXP = combatStats.ExperienceToNextLevel;
        textUI.text = currentXP + " / " + nextLvlXP;
    }
}
