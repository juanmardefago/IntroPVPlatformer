using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealthBarHandler : MonoBehaviour {

    public Image healthBar;
    public Image healthBarBottom;
    public EnemyCombatScript combatScript;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        healthBar.fillAmount = combatScript.HealthPercentage();
	}

    public void Disable()
    {
        healthBar.enabled = false;
        healthBarBottom.enabled = false;
    }
}
