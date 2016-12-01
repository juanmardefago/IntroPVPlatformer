using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootDropScript : MonoBehaviour {

    public int maxCoins;
    public int minCoins;

    public List<GameObject> dropList;
    public List<float> dropListChance;

    public GameObject coinPrefab;
    public GameObject genericItemPrefab;

    public GameObject healthPackPrefab;
    public float dropChanceHealthPack;

    public void DropLoot()
    {
        SpawnCoins();
        SpawnHealthPack();
        SpawnItems();
    }

    private void SpawnHealthPack()
    {
        if (Random.value <= dropChanceHealthPack)
        {
            GameObject healthPack = Instantiate(healthPackPrefab);
            healthPack.transform.position = transform.position;
            Rigidbody2D rBody = healthPack.GetComponent<Rigidbody2D>();
            rBody.isKinematic = false;
            rBody.AddForce(new Vector2(Random.Range(3, -3), 10), ForceMode2D.Impulse);
        }
    }

    private void SpawnCoins()
    {
        // Instantiate prefab
        GameObject coin = Instantiate(coinPrefab);
        coin.transform.position = transform.position;
        // Set max and min coins
        CoinScript coinScript = coin.GetComponent<CoinScript>();
        coinScript.minAmount = minCoins;
        coinScript.maxAmount = maxCoins;
        // Set velocity for rigidbody.
        Rigidbody2D rBody = coin.GetComponent<Rigidbody2D>();
        rBody.isKinematic = false;
        rBody.AddForce(new Vector2(Random.Range(3,-3), 10), ForceMode2D.Impulse);
    }

    private void SpawnItems()
    {
        for(int i = 0; i < dropList.Count; i++)
        {
            if(Random.value <= dropListChance[i])
            {
                SpawnItem(dropList[i]);
            }
        }
    }

    private void SpawnItem(GameObject itemToDrop)
    {
        // Instantiate prefab
        GameObject item = Instantiate(genericItemPrefab);
        item.transform.position = transform.position;
        // Set the item to drop
        ItemScript itemScript = item.GetComponent<ItemScript>();
        itemScript.item = Instantiate(itemToDrop);
        itemScript.RefreshItemSprite();
        // Set velocity for rigidbody.
        Rigidbody2D rBody = item.GetComponent<Rigidbody2D>();
        rBody.isKinematic = false;
        rBody.AddForce(new Vector2(Random.Range(3, -3), 10), ForceMode2D.Impulse);
    }
}
