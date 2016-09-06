using UnityEngine;
using System.Collections;

public class CoinScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Inventory>().AddCoins(Random.Range(1,5));
            Destroy(gameObject);
        }
    }
}
