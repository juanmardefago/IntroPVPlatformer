using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    // hay que hacerla private para la version final
    public int coins;

    public int Coins { get { return coins;  } }

    public List<GameObject> items;

    public GameObject[] eqWeapons;

    public WeaponScript currentWeapon;

    public BulletsHandler bulletHandler;
    public WeaponImageHandler weaponImage;

	// Use this for initialization
	void Start () {
        coins = 0;
        items = new List<GameObject>();
        if(eqWeapons[0] != null)
        {
            ChangeWeapon(0);
        } else if (eqWeapons[1] != null)
        {
            ChangeWeapon(1);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Weapon1") && eqWeapons[0] != null)
        {
            ChangeWeapon(0);
        }
        else if (Input.GetButtonDown("Weapon2") && eqWeapons[1] != null)
        {
            ChangeWeapon(1);
        }
    }

    public void AddCoins(int coinsToAdd)
    {
        coins += coinsToAdd;
    }

    public void SubtractCoins(int coinsToSubtract)
    {
        coins -= coinsToSubtract;
    }

    public void AddItem(GameObject item)
    {
        items.Add(item);
    }

    private void ChangeWeapon(int index)
    {
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }
        currentWeapon = eqWeapons[index].GetComponent<WeaponScript>();
        currentWeapon.gameObject.SetActive(true);
        bulletHandler.RefreshWeaponBullets();
        weaponImage.RefreshWeaponImage();
    }

    public List<string> GetWeaponsOwned()
    {
        List<string> res = new List<string>();
        foreach(GameObject item in items)
        {
            if(item.tag == "Weapon")
            {

                res.Add(item.GetComponent<WeaponScript>().weaponName);
            }
        }
        foreach(GameObject weapon in eqWeapons)
        {
            res.Add(weapon.GetComponent<WeaponScript>().weaponName);
        }
        return res;
    }
}
