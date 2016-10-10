using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpgradeNPCScript : UsableObjectScript
{
    public GameObject mainCanvas;
    public GameObject shopContent;
    public GameObject mainPanel;
    public GameObject buyPanel;
    public GameObject upgradePanel;
    public GameObject weaponRowPrefab;
    public Image buyWeaponImage;
    public Text buyWeaponText;

    public List<GameObject> weapons;
    public List<GameObject> upgrades;
    private List<GameObject> rows;

    public void Start()
    {
        rows = new List<GameObject>();
    }

    public override void DeInteract(PlayerNPCInteraction pi)
    {
        mainCanvas.SetActive(false);
        ResetPanels();
    }

    public override void Interact(PlayerNPCInteraction pi)
    {
        ListAllWeapons();
        mainCanvas.SetActive(true);
    }

    private void ListAllWeapons()
    {
        GameObject player = GameObject.FindWithTag("Player");
        List<string> weaponsOwned = player.GetComponent<Inventory>().GetWeaponsOwned();
        GameObject weaponRow;
        float offset = 0;
        foreach (GameObject weapon in weapons)
        {
            weaponRow = Instantiate(weaponRowPrefab);
            weaponRow.GetComponent<WeaponShopRowHandler>().RefreshWeapon(weapon);
            weaponRow.transform.SetParent(shopContent.transform, false);
            weaponRow.transform.localPosition = new Vector3(weaponRow.transform.localPosition.x, offset, weaponRow.transform.localPosition.z);
            offset -= 37;
            Button[] buttons = weaponRow.GetComponentsInChildren<Button>();
            AddOnClickListenersToButtons(weaponRow, weapon, buttons);
            if (weaponsOwned.Contains(weapon.GetComponent<WeaponScript>().weaponName))
            {
                buttons[0].interactable = false;
            }
            rows.Add(weaponRow);
        }
    }

    private void AddOnClickListenersToButtons(GameObject weaponRow, GameObject weapon, Button[] buttons)
    {
        // En teoría por el orden de los hijos, el primero tendría que ser el de compra, y el segundo el de upgrade, si hay algun bug es aca.
        // Lo implementé asi porque sino se tiene que ejecutar mucho codigo para checkear banda de cosas, y teniendo en cuenta que 
        // desde la v1 de unity esto te lo devuelve ordenado aunque no lo digan los doc, si se rompe ya sabemos donde es.

        // Los parametros del AddListener son funciones lambda de C#.
        foreach(Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
        buttons[0].onClick.AddListener(() => OpenBuyMenuForWeapon(weapon));
        buttons[1].onClick.AddListener(() => OpenUpgradeMenuForWeapon(weapon));
    }

    private void OpenBuyMenuForWeapon(GameObject weapon)
    {
        buyWeaponImage.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
        WeaponScript weaponScript = weapon.GetComponent<WeaponScript>();
        buyWeaponText.text = "Buy " + weaponScript.weaponName + " for " + weaponScript.weaponPrice + " coins?";
        Button[] buttons = buyPanel.GetComponentsInChildren<Button>();
        // Hay que limpiar los listeners antes de agregar, porque sino abriendo y cerrando la ventana varias veces
        // se agregan listeners cada vez que se la abre y entonces podes comprar más de 1 arma de un solo saque.
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
        Inventory playerInventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        // YesButton (por orden en escena).
        if (playerInventory.Coins >= weaponScript.weaponPrice)
        {
            buttons[0].onClick.AddListener(() => TriggerWeaponBuy(weapon, playerInventory));
            buttons[0].interactable = true;
        } else
        {
            buttons[0].interactable = false;
        }
        // NoButton
        buttons[1].onClick.AddListener(() => buyPanel.SetActive(false));
        buyPanel.SetActive(true);
    }

    private void OpenUpgradeMenuForWeapon(GameObject weapon)
    {
        mainPanel.SetActive(false);
        WeaponShopRowHandler weaponRowHeader = upgradePanel.GetComponentInChildren<WeaponShopRowHandler>();
        weaponRowHeader.RefreshWeapon(weapon);
        upgradePanel.SetActive(true);
    }

    private void TriggerWeaponBuy(GameObject weapon, Inventory playerInventory)
    {
        playerInventory.SubtractCoins(weapon.GetComponent<WeaponScript>().weaponPrice);
        playerInventory.AddItem(Instantiate(weapon));
        ResetPanels();
        ListAllWeapons();
    }

    private void ResetPanels()
    {
        foreach(GameObject row in rows)
        {
            Destroy(row);
        }
        rows.Clear();
        mainPanel.SetActive(true);
        buyPanel.SetActive(false);
        upgradePanel.SetActive(false);
    }

    public void GoBack()
    {
        mainPanel.SetActive(true);
        buyPanel.SetActive(false);
        upgradePanel.SetActive(false);
    }
}
