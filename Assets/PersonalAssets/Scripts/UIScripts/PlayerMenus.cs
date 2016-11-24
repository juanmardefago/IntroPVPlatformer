using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMenus : MonoBehaviour
{

    public GameObject mainPanel;
    public GameObject inventoryPanel;
    public GameObject equipPanel;
    public GameObject upgradePanel;
    public GameObject swapWeaponPanel;
    public GameObject buyPanel;
    public GameObject infoPanel;

    private bool isMenuOpen = false;
    private GameObject player;
    private Inventory inventory;
    public GameObject weaponRowPrefabSingleButton;

    // Rect transforms for content
    private RectTransform inventoryContent;
    private RectTransform swapWeaponContent;
    private RectTransform equippedGemsContent;
    private RectTransform availableGemsContent;

    private List<GameObject> rows;
    private List<GameObject> swapRows;
    private List<GameObject> gemRows;
    public Image buyWeaponImage;
    public Text buyWeaponText;
    public GameObject gemRowPrefab;
    public GameObject weaponRowPrefab;


    // Use this for initialization
    void Start()
    {
        rows = new List<GameObject>();
        swapRows = new List<GameObject>();
        gemRows = new List<GameObject>();
        player = GameObject.FindWithTag("Player");
        inventory = player.GetComponent<Inventory>();
        inventoryContent = inventoryPanel.GetComponent<ScrollRect>().content;
        swapWeaponContent = swapWeaponPanel.GetComponent<ScrollRect>().content;
        equippedGemsContent = upgradePanel.GetComponentsInChildren<ScrollRect>()[0].content;
        availableGemsContent = upgradePanel.GetComponentsInChildren<ScrollRect>()[1].content;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("PlayerMenu") && !isMenuOpen)
        {
            OpenPlayerMenu();
        }
        else if (Input.GetButtonDown("PlayerMenu") && isMenuOpen)
        {
            ClosePlayerMenu();
        }
    }

    public void ClosePlayerMenu()
    {
        isMenuOpen = false;
        mainPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        infoPanel.SetActive(false);
        equipPanel.SetActive(false);
        buyPanel.SetActive(false);
        swapWeaponPanel.SetActive(false);
        upgradePanel.SetActive(false);
        DestroyAllRows();
    }

    public void OpenPlayerMenu()
    {
        mainPanel.SetActive(true);
        isMenuOpen = true;
    }

    public void OpenInventory()
    {
        ListAllWeapons();
        ListEquipedWeapons();
        mainPanel.SetActive(false);
        inventoryPanel.SetActive(true);
        equipPanel.SetActive(true);
    }

    private void ListAllWeapons()
    {
        List<GameObject> weaponsOwned = inventory.GetWeaponsOwned();
        GameObject weaponRow;
        float offset = 0;
        foreach (GameObject weapon in weaponsOwned)
        {
            weaponRow = Instantiate(weaponRowPrefabSingleButton);
            weaponRow.GetComponent<WeaponShopRowHandler>().RefreshWeapon(weapon);
            weaponRow.transform.SetParent(inventoryContent, false);
            weaponRow.transform.localPosition = new Vector3(weaponRow.transform.localPosition.x, offset, weaponRow.transform.localPosition.z);
            offset -= 37;
            AddOnClickListenerToButton(weapon, weaponRow.GetComponentInChildren<Button>());
            rows.Add(weaponRow);
        }
        inventoryContent.sizeDelta = new Vector2(inventoryContent.sizeDelta.x, -offset);
    }

    private void AddOnClickListenerToButton(GameObject weapon, Button button)
    {
        // Los parametros del AddListener son funciones lambda de C#.
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OpenUpgradeMenuForWeapon(weapon));
        button.GetComponentInChildren<Text>().text = "Upgrade";
    }

    private void OpenUpgradeMenuForWeapon(GameObject weapon)
    {
        PlayerCombatScript combatScript = player.GetComponent<PlayerCombatScript>();
        inventoryPanel.SetActive(false);
        upgradePanel.GetComponentInChildren<WeaponShopRowHandler>().RefreshWeapon(weapon);
        Button lvlUpButton = upgradePanel.GetComponentInChildren<Button>();
        lvlUpButton.onClick.RemoveAllListeners();

        WeaponScript weaponScript = weapon.GetComponent<WeaponScript>();
        lvlUpButton.onClick.AddListener(() => OpenBuyMenuForWeapon(weapon, "Level up " + weaponScript.weaponName + " for " + weaponScript.weaponPriceToLevelUp + " coins?",
            () => TriggerWeaponLevelUp(weapon, combatScript.Level, weaponScript.weaponPriceToLevelUp, lvlUpButton), weaponScript.weaponPriceToLevelUp));

        if (weapon.GetComponent<WeaponScript>().weaponLevel >= combatScript.Level)
        {
            lvlUpButton.interactable = false;
        }
        else
        {
            lvlUpButton.interactable = true;
        }
        ReloadGems(weapon);
        upgradePanel.SetActive(true);
    }

    private void OpenBuyMenuForWeapon(GameObject weapon, string text, UnityEngine.Events.UnityAction action, int coinsNeeded)
    {
        buyWeaponImage.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
        buyWeaponText.text = text;
        Button[] buttons = buyPanel.GetComponentsInChildren<Button>();
        // Hay que limpiar los listeners antes de agregar, porque sino abriendo y cerrando la ventana varias veces
        // se agregan listeners cada vez que se la abre y entonces podes comprar más de 1 arma de un solo saque.
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
        // YesButton (por orden en escena).
        if (inventory.Coins >= coinsNeeded)
        {
            buttons[0].onClick.AddListener(action);
            buttons[0].interactable = true;
        }
        else
        {
            buttons[0].interactable = false;
        }
        // NoButton
        buttons[1].onClick.AddListener(() => buyPanel.SetActive(false));
        buyPanel.SetActive(true);
    }

    private void ListAllGems(GameObject weapon)
    {
        ListEquippedGems(weapon);
        ListAvailableGems(weapon);
    }

    private void ListEquippedGems(GameObject weapon)
    {
        List<GameObject> equippedGems = weapon.GetComponent<WeaponScript>().gems.ConvertAll((UpgradeScript u) => u.gameObject);
        float offset = 0;
        offset = ListGemsInPanel(weapon, equippedGemsContent, equippedGems, "Unequip", offset);
    }

    private void ListAvailableGems(GameObject weapon)
    {
        List<GameObject> unequippedGems = inventory.GetUnequippedGems();
        float offset = 0;
        offset = ListGemsInPanel(weapon, availableGemsContent, unequippedGems, "Equip", offset);
    }

    private float ListGemsInPanel(GameObject weapon, RectTransform rTransform, List<GameObject> gemList, string buttonText, float startingOffset = 0)
    {
        GameObject gemRow;
        float offset = startingOffset;
        foreach (GameObject gem in gemList)
        {
            gemRow = Instantiate(gemRowPrefab);
            GemRowHandler handler = gemRow.GetComponent<GemRowHandler>();
            handler.RefreshGem(gem);
            gemRow.transform.SetParent(rTransform, false);
            gemRow.transform.localPosition = new Vector3(gemRow.transform.localPosition.x, offset, gemRow.transform.localPosition.z);
            offset -= 37;
            // Obtengo los botones
            Button infoButton = handler.gemInfoButton;
            Button equipButton = handler.gemEquipButton;
            // Limpio los botones
            infoButton.onClick.RemoveAllListeners();
            equipButton.onClick.RemoveAllListeners();
            // Agrego el listener
            AddListenersToGemRowButtons(infoButton, equipButton, gem.GetComponent<UpgradeScript>().description, gem, weapon, buttonText);
            // Si no hay más espacio para equipar gemas y es una gema que se puede equipar, le desactivo el boton para equipar
            if (buttonText == "Equip" && !weapon.GetComponent<WeaponScript>().CanEquipGem())
            {
                equipButton.interactable = false;
            }
            else if (buttonText == "Equip")
            {
                equipButton.interactable = true;
            }
            gemRows.Add(gemRow);
        }
        rTransform.sizeDelta = new Vector2(equippedGemsContent.sizeDelta.x, -offset);
        return offset;
    }

    private void AddListenersToGemRowButtons(Button infoButton, Button equipButton, string infoText, GameObject gem, GameObject weapon, string equipButtonText)
    {
        infoButton.onClick.AddListener(() => ShowTextDialog(infoText, gem.GetComponent<UpgradeScript>().upgradeSprite));
        if (equipButtonText == "Equip")
        {
            equipButton.onClick.AddListener(() => EquipGemToWeapon(gem, weapon));
        }
        else if (equipButtonText == "Unequip")
        {
            equipButton.onClick.AddListener(() => UnequipGemFromWeapon(gem, weapon));
        }
        equipButton.GetComponentInChildren<Text>().text = equipButtonText;
    }

    private void ShowTextDialog(string text, Sprite sprite)
    {
        infoPanel.GetComponentInChildren<Text>().text = text;
        infoPanel.GetComponentsInChildren<Image>()[2].sprite = sprite;
        infoPanel.SetActive(true);
    }

    private void UnequipGemFromWeapon(GameObject gem, GameObject weapon)
    {
        inventory.UnequipGemFromWeapon(gem, weapon);
        ReloadGems(weapon);
    }

    private void EquipGemToWeapon(GameObject gem, GameObject weapon)
    {
        inventory.EquipGemToWeapon(gem, weapon);
        ReloadGems(weapon);
    }

    private void TriggerWeaponLevelUp(GameObject weapon, int playerLevel, int coinsToSubtract, Button myself)
    {
        inventory.SubtractCoins(coinsToSubtract);
        weapon.GetComponent<WeaponScript>().LevelUp();
        upgradePanel.GetComponentInChildren<WeaponShopRowHandler>().RefreshWeapon(weapon);
        if (weapon.GetComponent<WeaponScript>().weaponLevel >= playerLevel)
        {
            myself.interactable = false;
        }
        buyPanel.SetActive(false);
    }

    private void ListEquipedWeapons()
    {
        GameObject[] weaponsEquiped = inventory.GetEquippedWeapons();
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
        }
        else
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
        }
        else
        {
            unequipButton.interactable = false;
            // hago la imagen transparente
            weaponHeader.GetComponentsInChildren<Image>()[1].color = new Color(0, 0, 0, 0);
            // Pongo un texto para avisarle al usuario que no tiene equipada un arma en ese slot
            weaponHeader.GetComponentInChildren<Text>().text = "Choose a weapon to equip";
        }
        // Agregar las armas posibles para swapear
        ReloadSwapableWeapons(weapon, slot);
        // Habilitar el panel correspondiente
        swapWeaponPanel.SetActive(true);
        inventoryPanel.SetActive(false);

    }

    private void ListSwapableWeapons(GameObject weaponToSwap, int slot)
    {
        List<GameObject> unequippedWeapons = inventory.GetUnequippedWeapons();
        float offsetY = 0;
        GameObject weaponRow;
        foreach (GameObject weapon in unequippedWeapons)
        {
            weaponRow = Instantiate(weaponRowPrefabSingleButton);
            swapRows.Add(weaponRow);
            weaponRow.GetComponent<WeaponShopRowHandler>().RefreshWeapon(weapon);
            weaponRow.transform.SetParent(swapWeaponContent, false);
            weaponRow.transform.localPosition = new Vector3(weaponRow.transform.localPosition.x, offsetY, weaponRow.transform.localPosition.z);
            offsetY -= 37;
            Button button = weaponRow.GetComponentInChildren<Button>();
            if (weaponToSwap != null)
            {
                button.GetComponentInChildren<Text>().text = "Swap Weapon";
            }
            else
            {
                button.GetComponentInChildren<Text>().text = "Equip Weapon";
            }
            AddListenerToSwapWeaponButton(button, weapon, slot);
        }
        swapWeaponContent.sizeDelta = new Vector2(swapWeaponContent.sizeDelta.x, -offsetY);
    }

    private void ReloadSwapableWeapons(GameObject weaponToSwap, int slot)
    {
        foreach (GameObject row in swapRows)
        {
            Destroy(row);
        }
        ListSwapableWeapons(weaponToSwap, slot);
    }

    private void AddListenerToSwapWeaponButton(Button button, GameObject weapon, int slot)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => EquipOrSwap(weapon, slot));
    }

    private void EquipOrSwap(GameObject weapon, int slot)
    {
        inventory.EquipOrSwap(weapon, slot);
        GoBackFromSwapMenu();
    }

    private void Unequip(GameObject weapon)
    {
        inventory.Unequip(weapon);
        ListEquipedWeapons();
        GoBackFromSwapMenu();
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

    public void GoBack()
    {
        inventoryPanel.SetActive(true);
        swapWeaponPanel.SetActive(false);
        buyPanel.SetActive(false);
        upgradePanel.SetActive(false);
    }

    private void ReloadGems(GameObject weapon)
    {
        foreach (GameObject row in gemRows)
        {
            Destroy(row);
        }
        ListAllGems(weapon);
    }

    private void DestroyAllRows()
    {
        foreach (GameObject row in rows)
        {
            Destroy(row);
        }
        foreach (GameObject row in swapRows)
        {
            Destroy(row);
        }
        foreach (GameObject row in gemRows)
        {
            Destroy(row);
        }
    }

    public void GoBackDescription()
    {
        infoPanel.SetActive(false);
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }

    // Acá se tendría que hacer un save antes de salir ( o pedirle que lo haga, o agregar un boton de save y una advertencia)
    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

}
