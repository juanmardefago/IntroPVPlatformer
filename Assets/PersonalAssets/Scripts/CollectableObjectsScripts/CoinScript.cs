using UnityEngine;
using System.Collections;

public class CoinScript : MonoBehaviour
{
    public int minAmount;
    public int maxAmount;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Inventory>().AddCoins(Random.Range(minAmount, maxAmount));
            Destroy(gameObject);
        }
    }
}
