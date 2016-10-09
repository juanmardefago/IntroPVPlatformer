using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradeNPCScript : UsableObjectScript
{
    public GameObject mainCanvas;
    public GameObject shopContent;
    public GameObject weaponRowPrefab;

    public List<GameObject> weapons;
    public List<GameObject> upgrades;

    public void Start()
    {
        ListAllWeapons();
    }

    public override void DeInteract(PlayerNPCInteraction pi)
    {
        Debug.Log("Deje de interactuar con el NPC");
        mainCanvas.SetActive(false);
    }

    public override void Interact(PlayerNPCInteraction pi)
    {
        Debug.Log("Estoy interactuando con el NPC");
        mainCanvas.SetActive(true);
    }

    private void ListAllWeapons()
    {
        GameObject weaponRow;
        float offset = 0;
        foreach (GameObject weapon in weapons)
        {
            weaponRow = Instantiate(weaponRowPrefab);
            weaponRow.GetComponent<WeaponShopRowHandler>().RefreshWeapon(weapon);
            weaponRow.transform.SetParent(shopContent.transform, false);
            weaponRow.transform.localPosition = new Vector3(weaponRow.transform.localPosition.x, offset, weaponRow.transform.localPosition.z);
            offset -= 37;
        }
    }
}
