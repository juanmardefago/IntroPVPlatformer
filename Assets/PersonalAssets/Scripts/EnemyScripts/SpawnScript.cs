using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnScript : MonoBehaviour {

    public List<GameObject> terranEnemies;
    public List<GameObject> waterEnemies;
    public bool underwater;

    public void SpawnRandomEnemy(int level)
    {
        GameObject newEnemy;
        if (!underwater)
        {
            newEnemy = Instantiate(terranEnemies[NextInt(terranEnemies.Count - 1)]);
        } else
        {
            newEnemy = Instantiate(waterEnemies[NextInt(waterEnemies.Count - 1)]);
        }
        EnemyCombatScript combatScript = newEnemy.GetComponent<EnemyCombatScript>();
        combatScript.level = level;
        combatScript.AdjustStatsForLevel();
        newEnemy.transform.position = transform.position;
    }

    private int NextInt(int max)
    {
        return Mathf.RoundToInt(Random.value * max);
    }

    public void UnderwaterStatus(bool status)
    {
        underwater = status;
    }
}
