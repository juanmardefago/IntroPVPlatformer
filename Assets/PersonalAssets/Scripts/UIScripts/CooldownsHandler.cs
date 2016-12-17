using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CooldownsHandler : MonoBehaviour {

    public Image shieldCD;
    public Image chargedShotCD;
    public PlayerCombatScript combatScript;
	
	// Update is called once per frame
	void Update () {
        shieldCD.fillAmount = combatScript.ShieldCDFill();
        chargedShotCD.fillAmount = combatScript.ChargedShotCDFill();
	}
}
