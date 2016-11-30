using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public List<SpawnScript> possibleSpawns;
    private bool onCooldown;
    public int maxEnemiesToSpawnAtTheSameTime;


    public void OnTriggerStay2D(Collider2D other)
    {
        if(!onCooldown) StartCoroutine(SpawnRandomEnemy());
    }

    private IEnumerator SpawnRandomEnemy()
    {
        onCooldown = true;
        int enemiesToSpawn = NextInt(maxEnemiesToSpawnAtTheSameTime);
        for(; enemiesToSpawn > 0; enemiesToSpawn--)
        {
            possibleSpawns[NextInt(possibleSpawns.Count - 1)].SpawnRandomEnemy();
        }
        yield return new WaitForSeconds(Random.Range(2, 5));
        onCooldown = false;
    }

    private int NextInt(int max)
    {
        return Mathf.RoundToInt(Random.value * max);
    }
}
