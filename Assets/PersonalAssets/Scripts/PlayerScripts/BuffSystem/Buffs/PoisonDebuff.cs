using UnityEngine;
using System.Collections;

public class PoisonDebuff : BuffScript {

    public int poisonDamage;
    public float poisonCD;
    private Damagable combatScript;
    private Color oldColor;

    public override void ApplyBaseEffect(GameObject unit)
    {
        if (unit.tag == "Player")
        {
            combatScript = unit.GetComponent<PlayerCombatScript>();
        } else
        {
            combatScript = unit.GetComponent<EnemyCombatScript>();
        }
        InvokeRepeating("PoisonTick", 0, poisonCD);
        oldColor = unit.GetComponent<SpriteRenderer>().color;
        unit.GetComponent<SpriteRenderer>().color = Color.magenta;
    }

    public override void RemoveBaseEffect(GameObject unit)
    {
        CancelInvoke();
        unit.GetComponent<SpriteRenderer>().color = oldColor;
    }

    private void PoisonTick()
    {
        combatScript.TakeDamage(poisonDamage, Color.magenta);
    }
}
