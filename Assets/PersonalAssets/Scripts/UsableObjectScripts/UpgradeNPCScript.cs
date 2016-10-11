using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpgradeNPCScript : UsableObjectScript
{
    public GameObject mainCanvas;
    public GameObject shopContent;
    public GameObject mainPanel;
    public GameObject equipPanel;
    public GameObject swapWeaponPanel;
    public GameObject buyPanel;
    public GameObject upgradePanel;
    public GameObject weaponRowPrefab;
    public GameObject weaponRowPrefabSingleButton;
    public Image buyWeaponImage;
    public Text buyWeaponText;

    public List<GameObject> weapons;
    public List<GameObject> upgrades;
    private List<GameObject> rows;
    private List<GameObject> swapRows;

    public void Start()
    {
        rows = new List<GameObject>();
        swapRows = new List<GameObject>();
    }

    public override void DeInteract(PlayerNPCInteraction pi)
    {
        mainCanvas.SetActive(false);
        ResetPanels();
    }

    public override void Interact(PlayerNPCInteraction pi)
    {
        ListAllWeapons();
        ListEquipedWeapons();
        mainCanvas.SetActive(true);
    }

    private void ListAllWeapons()
    {
        Inventory inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        List<string> weaponsOwned = inventory.GetWeaponsOwned();
        GameObject weaponRow;
        float offset = 0;
        GameObject weaponToShow;
        foreach (GameObject weapon in weapons)
        {
            weaponRow = Instantiate(weaponRowPrefabSingleButton);
            weaponToShow = GetRealWeaponIfPossible(inventory, weapon);
            weaponRow.GetComponent<WeaponShopRowHandler>().RefreshWeapon(weaponToShow);
            weaponRow.transform.SetParent(shopContent.transform, false);
            weaponRow.transform.localPosition = new Vector3(weaponRow.transform.localPosition.x, offset, weaponRow.transform.localPosition.z);
            offset -= 37;
            AddOnClickListenerToButton(weaponToShow, weaponRow.GetComponentInChildren<Button>(), weaponsOwned.Contains(weapon.GetComponent<WeaponScript>().weaponName));
            rows.Add(weaponRow);
        }
        RectTransform rTransform = shopContent.GetComponent<RectTransform>();
        rTransform.sizeDelta = new Vector2(rTransform.sizeDelta.x, -offset);
    }

    private GameObject GetRealWeaponIfPossible(Inventory inventory, GameObject weapon)
    {
        GameObject possibleWeapon = inventory.GetWeaponWithName(weapon.GetComponent<WeaponScript>().weaponName);
        if(possibleWeapon == null)
        {
            possibleWeapon = weapon;
        }
        return possibleWeapon;
    }

    private void AddOnClickListenerToButton(GameObject weapon, Button button, bool playerOwnsTheWeapon)
    {
        // Los parametros del AddListener son funciones lambda de C#.
        button.onClick.RemoveAllListeners();
        if (playerOwnsTheWeapon)
        {
            button.onClick.AddListener(() => OpenUpgradeMenuForWeapon(weapon));
            button.GetComponentInChildren<Text>().text = "Upgrade";
        }
        else
        {
            button.onClick.AddListener(() => OpenBuyMenuForWeapon(weapon));
            button.GetComponentInChildren<Text>().text = "Buy";
        }
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
        PlayerCombatScript combatScript = GameObject.FindWithTag("Player").GetComponent<PlayerCombatScript>();
        mainPanel.SetActive(false);
        upgradePanel.GetComponentInChildren<WeaponShopRowHandler>().RefreshWeapon(weapon);
        Button lvlUpButton = upgradePanel.GetComponentInChildren<Button>();
        lvlUpButton.onClick.RemoveAllListeners();
        lvlUpButton.onClick.AddListener(() => OpenDialogLevelUpWeapon(weapon));

        if (weapon.GetComponent<WeaponScript>().weaponLevel >= combatScript.Level)
        {
            lvlUpButton.interactable = false;
        }
        else
        {
            lvlUpButton.interactable = true;
        }
        upgradePanel.SetActive(true);
    }

    private void OpenDialogLevelUpWeapon(GameObject weapon)
    {
        // Habria que agregar una especie de dialogo como la de buyWeapon que pregunte si quiere comprar la mejora
        // Además hay que agregar el precio de la mejora y la formula para calcular el 
        // proximo precio de mejora.
        weapon.GetComponent<WeaponScript>().LevelUp();
        upgradePanel.GetComponentInChildren<WeaponShopRowHandler>().RefreshWeapon(weapon);
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
        foreach (GameObject row in swapRows)
        {
            Destroy(row);
        }
        rows.Clear();
        swapRows.Clear();
        mainPanel.SetActive(true);
        swapWeaponPanel.SetActive(false);
        buyPanel.SetActive(false);
        upgradePanel.SetActive(false);
    }

    private void ListEquipedWeapons()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject[] weaponsEquiped = player.GetComponent<Inventory>().GetEquippedWeapons();
        Button[] buttons = equipPanel.GetComponentsInChildren<Button>();
        // Weapon 1 - Imagen y texto del child (la [0] es la del boton)
        ConfigButtonForEquipWeapon(buttons[0], weaponsEquiped[0], 0);
        // Weapon 2
        ConfigButtonForEquipWeapon(buttons[1], weaponsEquiped[1], 1);
    }

    private void ConfigButtonForEquipWeapon(Button button, GameObject weapon, int slot)
    {
        if (weapon != null)
        {
            button.GetComponentsInChildren<Image>()[1].color = Color.white;
            button.GetComponentsInChildren<Image>()[1].sprite = weapon.GetComponent<SpriteRenderer>().sprite;
            button.GetComponentInChildren<Text>().text = weapon.GetComponent<WeaponScript>().weaponName;            
        } else
        {
            button.GetComponentsInChildren<Image>()[1].color = new Color(0, 0, 0, 0);
            button.GetComponentInChildren<Text>().text = "Click to equip a weapon";
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OpenSwapMenuForWeapon(weapon, slot));
    }

    private void OpenSwapMenuForWeapon(GameObject weapon, int slot)
    {
        Button unequipButton = swapWeaponPanel.GetComponentInChildren<Button>();
        GameObject weaponHeader = swapWeaponPanel.GetComponentInChildren<WeaponShopRowHandler>().gameObject;
        unequipButton.onClick.RemoveAllListeners();
        if (weapon != null)
        {
            // Agregarle la funcionalidad al unequipButton
            unequipButton.interactable = true;
            unequipButton.onClick.AddListener(() => Unequip(weapon));
            // Agrego la foto a la imagen correspondiente, que por la estructura de escena es la 2da del array ([1])
            weaponHeader.GetComponentsInChildren<Image>()[1].color = Color.white;
            weaponHeader.GetComponentsInChildren<Image>()[1].sprite = weapon.GetComponent<SpriteRenderer>().sprite;
            // Agrego el texto del arma
            weaponHeader.GetComponentInChildren<Text>().text = weapon.GetComponent<WeaponScript>().weaponName;
        } else
        {
            unequipButton.interactable = false;
            // hago la imagen transparente
            weaponHeader.GetComponentsInChildren<Image>()[1].color = new Color(0, 0, 0, 0);
            // Pongo un texto para avisarle al usuario que no tiene equipada un arma en ese slot
            weaponHeader.GetComponentInChildren<Text>().text = "Choose a weapon to equip";
        }
        // Agregar las armas posibles para swapear
        ListSwapableWeapons(weapon, slot);
        // Habilitar el panel correspondiente
        swapWeaponPanel.SetActive(true);
        mainPanel.SetActive(false);

    }

    private void ListSwapableWeapons(GameObject weaponToSwap, int slot)
    {
        List<GameObject> unequippedWeapons = GameObject.FindWithTag("Player").GetComponent<Inventory>().GetUnequippedWeapons();
        float offsetY = -82;
        GameObject weaponRow;
        foreach (GameObject weapon in unequippedWeapons)
        {
            weaponRow = Instantiate(weaponRowPrefabSingleButton);
            swapRows.Add(weaponRow);
            weaponRow.GetComponent<WeaponShopRowHandler>().RefreshWeapon(weapon);
            weaponRow.transform.SetParent(swapWeaponPanel.transform, false);
            weaponRow.transform.localPosition = new Vector3(weaponRow.transform.localPosition.x, offsetY, weaponRow.transform.localPosition.z);
            offsetY -= 37;
            Button button = weaponRow.GetComponentInChildren<Button>();
            if (weaponToSwap != null) {
                button.GetComponentInChildren<Text>().text = "Swap Weapon";
            }
            else {
                button.GetComponentInChildren<Text>().text = "Equip Weapon";
            }
            AddListenerToSwapWeaponButton(button, weapon, slot);
        }
    }

    private void AddListenerToSwapWeaponButton(Button button, GameObject weapon, int slot)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => EquipOrSwap(weapon, slot));
    }

    private void EquipOrSwap(GameObject weapon, int slot)
    {
        GameObject.FindWithTag("Player").GetComponent<Inventory>().EquipOrSwap(weapon, slot);
        GoBackFromSwapMenu();
    }

    private void Unequip(GameObject weapon)
    {
        GameObject player = GameObject.FindWithTag("Player");
        Inventory inventory = player.GetComponent<Inventory>();
        inventory.Unequip(weapon);
        ListEquipedWeapons();
        GoBackFromSwapMenu();
    }

    public void GoBack()
    {
        mainPanel.SetActive(true);
        swapWeaponPanel.SetActive(false);
        buyPanel.SetActive(false);
        upgradePanel.SetActive(false);
    }

    public void GoBackFromSwapMenu()
    {
        ListEquipedWeapons();
        foreach (GameObject row in swapRows)
        {
            Destroy(row);
        }
        swapRows.Clear();
        GoBack();
    }
    public void GoBackFromUpgradeMenu()
    {
        ResetPanels();
        ListAllWeapons();
    }
}
