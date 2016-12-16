using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyNameHandler : MonoBehaviour {

    public EnemyCombatScript combatScript;
    public Text textBox;

	// Use this for initialization
	void Start () {
        textBox.text = combatScript.enemyName + " Lv. " + (combatScript.level + 1);
	}
	

    public void Disable()
    {
        textBox.enabled = false;
    }
}
