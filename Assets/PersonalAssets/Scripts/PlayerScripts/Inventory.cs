using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    // hay que hacerla private para la version final
    public int coins;

    public int Coins { get { return coins;  } }

    public List<GameObject> items;

    public GameObject weapon1;
    public GameObject weapon2;

    public WeaponScript currentWeapon;

    public BulletsHandler bulletHandler;
    public WeaponImageHandler weaponImage;
    public Transform inventoryBag;
    private int currentWeaponSlot;

	// Use this for initialization
	void Start () {
        coins = 0;
        currentWeaponSlot = -1;
        items = new List<GameObject>();
        InitilizeEquippedWeapons();
    }

    private void InitilizeEquippedWeapons()
    {
        if (weapon1 != null)
        {
            ChangeWeapon(0);
        }
        else if (weapon2 != null)
        {
            ChangeWeapon(1);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Weapon1") && weapon1 != currentWeapon)
        {
            ChangeWeapon(0);
        }
        else if (Input.GetButtonDown("Weapon2") && weapon2 != currentWeapon)
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
        WeaponScript possibleWScript = item.GetComponent<WeaponScript>();
        if(possibleWScript != null)
        {
            if(weapon1 == null)
            {
                if(currentWeapon != null)
                {
                    EquipOrSwapSneaky(item, 0);
                }
                else
                {
                    EquipOrSwap(item, 0);
                }
            }
            else if (weapon2 == null)
            {
                if (currentWeapon != null)
                {
                    EquipOrSwapSneaky(item, 1);
                }
                else
                {
                    EquipOrSwap(item, 1);
                }
            }
        }
    }

    public void RemoveItem(GameObject item)
    {
        items.Remove(item);
        Destroy(item);
    }

    private void ChangeWeapon(int index)
    {
        // If current weapon is not null then i have to deactivate it, effectively putting it away.
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }

        switch (index)
        {
            case 0:
                ChangeToWeapon1();
                break;
            case 1:
                ChangeToWeapon2();
                break;
            default:
                break;
        }

        bulletHandler.RefreshWeaponBullets();
        weaponImage.RefreshWeaponImage();
    }

    private void ChangeToWeapon1()
    {
        if (weapon1 != null)
        {
            currentWeapon = weapon1.GetComponent<WeaponScript>();
            currentWeapon.gameObject.SetActive(true);
            currentWeaponSlot = 0;
        }
        else
        {
            currentWeapon = null;
            currentWeaponSlot = -1;
        }
    }

    private void ChangeToWeapon2()
    {
        if (weapon2 != null)
        {
            currentWeapon = weapon2.GetComponent<WeaponScript>();
            currentWeapon.gameObject.SetActive(true);
            currentWeaponSlot = 0;
        }
        else
        {
            currentWeapon = null;
            currentWeaponSlot = -1;
        }
    }

    public List<GameObject> GetWeaponsOwned()
    {
        List<GameObject> res = GetEquippedWeapons();
        foreach (GameObject item in items)
        {
            if(item.tag == "Weapon")
            {

                res.Add(item);
            }
        }
        return res;
    }

    public List<GameObject> GetEquippedWeapons()
    {
        List<GameObject> res = new List<GameObject>();
        if (weapon1 != null)
        {
            res.Add(weapon1);
        }
        if (weapon2 != null)
        {
            res.Add(weapon2);
        }
        return res;
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
        if(weapon1 == weaponToUnequip)
        {
            UnequipAndPutInBag(weapon1, 0);
        } else if(weapon2 = weaponToUnequip)
        {
            UnequipAndPutInBag(weapon2, 1);
        }
    }

    private void UnequipAndPutInBag(GameObject weapon, int index)
    {
        WeaponScript wScript = weapon.GetComponent<WeaponScript>();
        SetWeaponBySlot(null, index);
        items.Add(weapon);
        if(wScript.weaponName == currentWeapon.weaponName)
        {
            ChangeWeapon((index+1) % 2);
        }
        weapon.SetActive(false);
        weapon.transform.SetParent(inventoryBag);
    }

    public void EquipOrSwap(GameObject weapon, int slot)
    {
        EquipOrSwapSneaky(weapon, slot);
        ChangeWeapon(slot);
    }


    public void EquipOrSwapSneaky(GameObject weapon, int slot)
    {
        if (GetWeaponBySlot(slot) != null)
        {
            Unequip(GetWeaponBySlot(slot));
        }
        SetWeaponBySlot(weapon, slot);
        weapon.transform.SetParent(transform);
        items.Remove(weapon);
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
        if (weapon1 != null && weapon1.GetComponent<WeaponScript>().weaponName == name)
        {
            res = weapon1;
        }
        if (weapon2 != null && weapon2.GetComponent<WeaponScript>().weaponName == name)
        {
            res = weapon2;
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

    public bool IsWeaponEquipped(GameObject weapon)
    {
        return weapon1 == weapon || weapon2 == weapon;
    }

    private GameObject GetWeaponBySlot(int slot)
    {
        GameObject res = weapon1;
        if(slot == 1)
        {
            res = weapon2;
        } else if(slot == -1)
        {
            res = null;
        }
        return res;
    }

    private void SetWeaponBySlot(GameObject weapon, int slot)
    {
        switch (slot)
        {
            case 0:
                weapon1 = weapon;
                break;
            case 1:
                weapon2 = weapon;
                break;
            default:
                break;
        }
    }
}
