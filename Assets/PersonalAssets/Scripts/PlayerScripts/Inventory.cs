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
    public Transform inventoryBag;
    private int currentWeaponSlot;

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
        item.transform.SetParent(inventoryBag, false);
        item.SetActive(false);
    }

    public void RemoveItem(GameObject item)
    {
        items.Remove(item);
        Destroy(item);
    }

    private void ChangeWeapon(int index)
    {
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }

        if (eqWeapons[index] != null)
        {
            currentWeapon = eqWeapons[index].GetComponent<WeaponScript>();
            currentWeapon.gameObject.SetActive(true);
            currentWeaponSlot = index;
        } else
        {
            currentWeapon = null;
            currentWeaponSlot = -1;
        }

        bulletHandler.RefreshWeaponBullets();
        weaponImage.RefreshWeaponImage();
    }

    public List<GameObject> GetWeaponsOwned()
    {
        List<GameObject> res = new List<GameObject>();
        foreach(GameObject item in items)
        {
            if(item.tag == "Weapon")
            {

                res.Add(item);
            }
        }
        foreach(GameObject weapon in eqWeapons)
        {
            if (weapon != null)
            {
                res.Add(weapon);
            }
        }
        return res;
    }

    public GameObject[] GetEquippedWeapons()
    {
        return eqWeapons;
    }

    public List<GameObject> GetUnequippedWeapons()
    {
        List<GameObject> res = new List<GameObject>();
        foreach(GameObject item in items)
        {
            if(item.tag == "Weapon")
            {
                res.Add(item);
            }
        }
        return res;
    }

    public void Unequip(GameObject weaponToUnequip)
    {
        for(int i = 0; i < 2; i++)
        {
            if(eqWeapons[i] == weaponToUnequip)
            {
                UnequipAndPutInBag(eqWeapons[i], i);
            }
        }
    }

    private void UnequipAndPutInBag(GameObject weapon, int index)
    {
        eqWeapons[index] = null;
        items.Add(weapon);
        if(weapon.GetComponent<WeaponScript>().weaponName == currentWeapon.weaponName)
        {
            ChangeWeapon((index+1) % 2);
        }
        weapon.SetActive(false);
        weapon.transform.SetParent(inventoryBag);
    }

    public void EquipOrSwap(GameObject weapon, int slot)
    {
        if(eqWeapons[slot] != null)
        {
            Unequip(eqWeapons[slot]);
        }
        eqWeapons[slot] = weapon;
        weapon.transform.SetParent(transform);
        items.Remove(weapon);
        ChangeWeapon(slot);
    }

    public GameObject GetWeaponWithName(string name)
    {
        GameObject res = null;
        foreach (GameObject item in items)
        {
            if (item.tag == "Weapon" && item.GetComponent<WeaponScript>().weaponName == name)
            {
                res = item;
            }
        }
        foreach (GameObject weapon in eqWeapons)
        {
            if (weapon != null && weapon.GetComponent<WeaponScript>().weaponName == name)
            {
                res = weapon;
            }
        }
        return res;
    }

    public List<GameObject> GetUnequippedGems()
    {
        List<GameObject> gems = new List<GameObject>();
        foreach(GameObject item in items)
        {
            if(item.tag == "Gem")
            {
                gems.Add(item);
            }
        }
        return gems;
    }

    public void EquipGemToWeapon(GameObject gem, GameObject weapon)
    {
        gem.transform.SetParent(weapon.transform);
        weapon.GetComponent<WeaponScript>().AddGem(gem);
        items.Remove(gem);
    }

    public void UnequipGemFromWeapon(GameObject gem, GameObject weapon)
    {
        AddItem(gem);
        weapon.GetComponent<WeaponScript>().RemoveGem(gem);
    }

    public int GetCurrentWeaponSlot()
    {
        return currentWeaponSlot;
    }
}
