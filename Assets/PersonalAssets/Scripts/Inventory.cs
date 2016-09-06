using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

    public int coins;

    public int Coins { get { return coins;  } }

	// Use this for initialization
	void Start () {
        coins = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddCoins(int coinsToAdd)
    {
        coins += coinsToAdd;
    }
}
