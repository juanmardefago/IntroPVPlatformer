using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExperienceHandler : MonoBehaviour {

    public GameObject player;
    private PlayerCombatScript combatStats;
    private Image experienceBar;
    private Text textUI;

    // Use this for initialization
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        combatStats = player.GetComponent<PlayerCombatScript>();
        experienceBar = gameObject.GetComponent<Image>();
        experienceBar.fillAmount = 1;
        textUI = GetComponentInChildren<Text>();
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
        experienceBar.fillAmount = (float) currentXP / nextLvlXP;
        textUI.text = currentXP + " / " + nextLvlXP;
    }
}
