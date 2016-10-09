using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    public int coins;

    public int Coins { get { return coins;  } }

    public List<GameObject> items;

    public GameObject currentWeapon;

	// Use this for initialization
	void Start () {
        coins = 0;
        items = new List<GameObject>();
        currentWeapon = GetComponentInChildren<WeaponScript>().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddCoins(int coinsToAdd)
    {
        coins += coinsToAdd;
    }

    public void AddItem(GameObject item)
    {
        items.Add(item);
    }

    public void EquipWeapon(int num)
    {
        int tempNum = num;
        int index = 0;
        while(tempNum > 0 && index < items.Count)
        {
            if (items[index] != null && items[index].tag == "Weapon")
            {
                tempNum--;
            }
            index++;
        }
        if (items[index] != null) {
            TriggerWeaponSwap(items[index]);
        }
    }

    private void TriggerWeaponSwap(GameObject weapon)
    {

    }
}
