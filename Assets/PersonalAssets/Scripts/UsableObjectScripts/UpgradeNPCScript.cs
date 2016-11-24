using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class UpgradeNPCScript : UsableObjectScript
{
    private GameObject player;
    private Inventory inventory;

    public GameObject mainCanvas;
    public GameObject buyWeaponsPanel;
    public GameObject buyPanel;
    public GameObject buyGemsPanel;
    public GameObject infoPanel;
    public GameObject topBar;
    public GameObject gemRowPrefab;
    public GameObject weaponRowPrefab;
    public GameObject weaponRowPrefabSingleButton;
    public Image buyWeaponImage;
    public Text buyWeaponText;

    public List<GameObject> weapons;
    public List<GameObject> upgrades;
    private List<GameObject> rows;
    private List<GameObject> swapRows;
    private List<GameObject> gemRows;

    // Content transforms to fill the scrollable lists.

    private RectTransform shopContent;
    private RectTransform availableGemsContent;


    public void Start()
    {
        rows = new List<GameObject>();
        swapRows = new List<GameObject>();
        gemRows = new List<GameObject>();
        shopContent = buyWeaponsPanel.GetComponent<ScrollRect>().content;
        availableGemsContent = buyGemsPanel.GetComponentInChildren<ScrollRect>().content;
        player = GameObject.FindWithTag("Player");
        inventory = player.GetComponent<Inventory>();
    }

    // Interaction methods, only called when player presses Interact (by default E) near an NPC
    public override void DeInteract(PlayerNPCInteraction pi)
    {
        CloseShopMenu();
    }

    public override void Interact(PlayerNPCInteraction pi)
    {
        mainCanvas.SetActive(true);
    }

    public void CloseShopMenu()
    {
        mainCanvas.SetActive(false);
        ResetPanels();
    }

    public void OpenBuyWeaponsMenu()
    {
        ResetPanels();
        ReloadWeapons();
        buyWeaponsPanel.SetActive(true);
    }

    public void OpenBuyGemMenu()
    {
        ResetPanels();
        ReloadGems();
        buyGemsPanel.SetActive(true);
    }

    // Lists all weapons that can be purchased on the mainPanel content (on a scrollable list).
    private void ListAllWeapons()
    {
        GameObject weaponRow;
        float offset = 0;
        foreach (GameObject weapon in weapons)
        {
            weaponRow = Instantiate(weaponRowPrefabSingleButton);
            weaponRow.GetComponent<WeaponShopRowHandler>().RefreshWeapon(weapon);
            weaponRow.transform.SetParent(shopContent, false);
            weaponRow.transform.localPosition = new Vector3(weaponRow.transform.localPosition.x, offset, weaponRow.transform.localPosition.z);
            offset -= 37;
            AddOnClickListenerToButton(weapon, weaponRow.GetComponentInChildren<Button>());
            rows.Add(weaponRow);
        }
        shopContent.sizeDelta = new Vector2(shopContent.sizeDelta.x, -offset);
    }

    // Le agrega el Listener a el boton "button" dependiendo de si el player ya tiene o no el arma
    // Se usa en el scrollable del main panel.
    // Si tiene el arma se agrega el boton de upgrade, sino el de buy.
    private void AddOnClickListenerToButton(GameObject weapon, Button button)
    {
        // Los parametros del AddListener son funciones lambda de C#.
        button.onClick.RemoveAllListeners();
        WeaponScript weaponScript = weapon.GetComponent<WeaponScript>();
        button.onClick.AddListener(() => OpenBuyMenuForWeapon(weapon, "Buy " + weaponScript.weaponName + " for " + weaponScript.weaponPrice + " coins?",
            () => TriggerWeaponBuy(weapon), weaponScript.weaponPrice));
        button.GetComponentInChildren<Text>().text = "Buy";
    }

    // Abre el Dialog (una ventanita que dice un texto y tiene 2 botones, SI y NO.
    // Lo hice lo mas generico posible porque lo reutilizo en otro panel, usa lambdas (UnityAction) y muchos parametros para personalizarlo.
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
        } else
        {
            buttons[0].interactable = false;
        }
        // NoButton
        buttons[1].onClick.AddListener(() => buyPanel.SetActive(false));
        buyPanel.SetActive(true);
    }

    private void ListGems()
    {
        float offset = 0;
        offset = ListGemsInPanel(availableGemsContent, upgrades, "Buy", offset);
    }

    private float ListGemsInPanel(RectTransform rTransform, List<GameObject> gemList, string buttonText, float startingOffset = 0)
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
            AddListenersToGemRowButtons(infoButton, equipButton, gem.GetComponent<UpgradeScript>().description, gem, buttonText);
            gemRows.Add(gemRow);
        }
        rTransform.sizeDelta = new Vector2(rTransform.sizeDelta.x, -offset);
        return offset;
    }

    private void AddListenersToGemRowButtons(Button infoButton, Button equipButton, string infoText, GameObject gem, string equipButtonText)
    {
        infoButton.onClick.AddListener(() => ShowTextDialog(infoText, gem.GetComponent<UpgradeScript>().upgradeSprite));
        if(equipButtonText == "Buy")
        {
            equipButton.onClick.AddListener(() => OpenBuyMenuForGem(gem));
        }
        equipButton.GetComponentInChildren<Text>().text = equipButtonText;
    }

    private void ShowTextDialog(string text, Sprite sprite)
    {
        infoPanel.GetComponentInChildren<Text>().text = text;
        infoPanel.GetComponentsInChildren<Image>()[2].sprite = sprite;
        infoPanel.SetActive(true);
    }

    private void BuyGem(GameObject gem)
    {
        inventory.SubtractCoins(gem.GetComponent<UpgradeScript>().upgradePrice);
        inventory.AddItem(Instantiate(gem));
        buyPanel.SetActive(false);
    }

    private void OpenBuyMenuForGem(GameObject gem)
    {
        UpgradeScript gemScript = gem.GetComponent<UpgradeScript>();
        buyWeaponImage.sprite = gemScript.upgradeSprite;
        buyWeaponText.text = "Buy " + gemScript.upgradeName + " for " + gemScript.upgradePrice + " coins?";
        Button[] buttons = buyPanel.GetComponentsInChildren<Button>();
        // Hay que limpiar los listeners antes de agregar, porque sino abriendo y cerrando la ventana varias veces
        // se agregan listeners cada vez que se la abre y entonces podes comprar más de 1 arma de un solo saque.
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
        // YesButton (por orden en escena).
        if (inventory.Coins >= gemScript.upgradePrice)
        {
            buttons[0].onClick.AddListener(() => BuyGem(gem));
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

    private void TriggerWeaponLevelUp(GameObject weapon, int playerLevel, int coinsToSubtract, Button myself)
    {
        inventory.SubtractCoins(coinsToSubtract);
        weapon.GetComponent<WeaponScript>().LevelUp();
        buyGemsPanel.GetComponentInChildren<WeaponShopRowHandler>().RefreshWeapon(weapon);
        if (weapon.GetComponent<WeaponScript>().weaponLevel >= playerLevel)
        {
            myself.interactable = false;
        }
        buyPanel.SetActive(false);
    }

    private void TriggerWeaponBuy(GameObject weapon)
    {
        inventory.SubtractCoins(weapon.GetComponent<WeaponScript>().weaponPrice);
        inventory.AddItem(Instantiate(weapon));
        buyPanel.SetActive(false);
    }

    // Metodos utiles para cambiar el estado de los paneles y limpiar listas
    private void ResetPanels()
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
        rows.Clear();
        swapRows.Clear();
        topBar.SetActive(true);
        buyWeaponsPanel.SetActive(false);
        buyPanel.SetActive(false);
        buyGemsPanel.SetActive(false);
    }

    public void GoBack()
    {
        buyWeaponsPanel.SetActive(true);
        buyPanel.SetActive(false);
        buyGemsPanel.SetActive(false);
    }

    public void GoBackFromUpgradeMenu()
    {
        ResetPanels();
        ListAllWeapons();
    }

    public void GoBackDescription()
    {
        infoPanel.SetActive(false);
    }

    private void GoBackAfterGemPurchase()
    {
        buyPanel.SetActive(false);
        ReloadGems();
    }

    private void ReloadGems()
    {
        foreach (GameObject row in gemRows)
        {
            Destroy(row);
        }
        ListGems();
    }

    private void ReloadWeapons()
    {
        foreach (GameObject row in rows)
        {
            Destroy(row);
        }
        ListAllWeapons();
    }
}
