using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelHandler : MonoBehaviour {

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
        HandleLevel();
    }

    private void HandleLevel()
    {
        int level = combatStats.Level;
        textUI.text = level.ToString();
    }
}
