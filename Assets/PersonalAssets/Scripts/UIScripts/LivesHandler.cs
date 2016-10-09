using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LivesHandler : MonoBehaviour {

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
        HandleLives();
    }

    private void HandleLives()
    {
        int lives = combatStats.lives;
        textUI.text = lives.ToString() ;
    }
}
