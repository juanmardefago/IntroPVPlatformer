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
    public GameObject sellWeaponsPanel;
    public GameObject buyGemsPanel;
    public GameObject sellGemsPanel;
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

    private RectTransform buyWeaponsContent;
    private RectTransform buyGemsContent;
    private RectTransform sellWeaponsContent;
    private RectTransform sellGemsContent;


    public void Start()
    {
        rows = new List<GameObject>();
        swapRows = new List<GameObject>();
        gemRows = new List<GameObject>();
        buyWeaponsContent = buyWeaponsPanel.GetComponent<ScrollRect>().content;
        sellWeaponsContent = sellWeaponsPanel.GetComponent<ScrollRect>().content;
        buyGemsContent = buyGemsPanel.GetComponent<ScrollRect>().content;
        sellGemsContent = sellGemsPanel.GetComponent<ScrollRect>().content;
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
        Time.timeScale = 0;
    }

    public void CloseShopMenu()
    {
        mainCanvas.SetActive(false);
        ResetPanels();
        Time.timeScale = 1;
    }

    public void OpenBuyWeaponsMenu()
    {
        ResetPanels();
        ReloadWeapons(buyWeaponsContent, weapons, (weapon, button) => AddOnClickListenerToButtonForBuy(weapon, button));
        buyWeaponsPanel.SetActive(true);
    }

    public void OpenBuyGemMenu()
    {
        ResetPanels();
        ReloadGems(buyGemsContent, upgrades, (infoButton, equipButton, infoText, gem) => AddListenersToGemRowButtonsForBuy(infoButton, equipButton, infoText, gem));
        buyGemsPanel.SetActive(true);
    }

    public void OpenSellWeaponsMenu()
    {
        ResetPanels();
        ReloadWeapons(sellWeaponsContent, inventory.GetUnequippedWeapons(), (weapon, button) => AddOnClickListenerToButtonForSell(weapon, button));
        sellWeaponsPanel.SetActive(true);
    }

    public void OpenSellGemsMenu()
    {
        ResetPanels();
        ReloadGems(sellGemsContent, inventory.GetUnequippedGems(), (infoButton, equipButton, infoText, gem) => AddListenersToGemRowButtonsForSell(infoButton, equipButton, infoText, gem));
        sellGemsPanel.SetActive(true);
    }

    // Lists all weapons that can be purchased on the mainPanel content (on a scrollable list).
    private void ListAllWeapons(RectTransform contentHolder, List<GameObject> weaponList, UnityEngine.Events.UnityAction<GameObject, Button> onClickAdder)
    {
        GameObject weaponRow;
        float offset = 0;
        foreach (GameObject weapon in weaponList)
        {
            weaponRow = Instantiate(weaponRowPrefabSingleButton);
            weaponRow.GetComponent<WeaponShopRowHandler>().RefreshWeapon(weapon);
            weaponRow.transform.SetParent(contentHolder, false);
            weaponRow.transform.localPosition = new Vector3(weaponRow.transform.localPosition.x, offset, weaponRow.transform.localPosition.z);
            offset -= 37;
            onClickAdder(weapon, weaponRow.GetComponentInChildren<Button>());
            rows.Add(weaponRow);
        }
        contentHolder.sizeDelta = new Vector2(contentHolder.sizeDelta.x, -offset);
    }

    // Le agrega el Listener a el boton "button" dependiendo de si el player ya tiene o no el arma
    // Se usa en el scrollable del main panel.
    // Si tiene el arma se agrega el boton de upgrade, sino el de buy.
    private void AddOnClickListenerToButtonForBuy(GameObject weapon, Button button)
    {
        // Los parametros del AddListener son funciones lambda de C#.
        button.onClick.RemoveAllListeners();
        WeaponScript weaponScript = weapon.GetComponent<WeaponScript>();
        button.onClick.AddListener(() => OpenBuyMenuForWeapon(weapon, "Buy " + weaponScript.weaponName + " for " + weaponScript.weaponPrice + " coins?",
            () => TriggerWeaponBuy(weapon), weaponScript.weaponPrice));
        button.GetComponentInChildren<Text>().text = "Buy";
    }

    private void AddOnClickListenerToButtonForSell(GameObject weapon, Button button)
    {
        // Los parametros del AddListener son funciones lambda de C#.
        button.onClick.RemoveAllListeners();
        WeaponScript weaponScript = weapon.GetComponent<WeaponScript>();
        button.onClick.AddListener(() => OpenBuyMenuForWeapon(weapon, "Sell " + weaponScript.weaponName + " for " + weaponScript.weaponPrice / 2 + " coins?",
            () => TriggerWeaponSell(weapon), 0));
        button.GetComponentInChildren<Text>().text = "Sell";
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

    private void ListGems(RectTransform contentHolder, List<GameObject> gemsList, UnityEngine.Events.UnityAction<Button, Button, string, GameObject> onClickAdder)
    {
        float offset = 0;
        offset = ListGemsInPanel(contentHolder, gemsList, onClickAdder, offset);
    }

    private float ListGemsInPanel(RectTransform rTransform, List<GameObject> gemList, UnityEngine.Events.UnityAction<Button, Button, string, GameObject> onClickAdder, float startingOffset = 0)
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
            onClickAdder(infoButton, equipButton, gem.GetComponent<UpgradeScript>().description, gem);
            gemRows.Add(gemRow);
        }
        rTransform.sizeDelta = new Vector2(rTransform.sizeDelta.x, -offset);
        return offset;
    }

    private void AddListenersToGemRowButtonsForBuy(Button infoButton, Button equipButton, string infoText, GameObject gem)
    {
        UpgradeScript gemScript = gem.GetComponent<UpgradeScript>();
        infoButton.onClick.AddListener(() => ShowTextDialog(infoText, gemScript.upgradeSprite));
        equipButton.onClick.AddListener(() => OpenBuyMenuForGem(gem, "Buy " + gemScript.upgradeName + " for " + gemScript.upgradePrice + " coins?", () => BuyGem(gem), gemScript.upgradePrice));
        equipButton.GetComponentInChildren<Text>().text = "Buy";
    }

    private void AddListenersToGemRowButtonsForSell(Button infoButton, Button equipButton, string infoText, GameObject gem)
    {
        UpgradeScript gemScript = gem.GetComponent<UpgradeScript>();
        infoButton.onClick.AddListener(() => ShowTextDialog(infoText, gemScript.upgradeSprite));
        equipButton.onClick.AddListener(() => OpenBuyMenuForGem(gem, "Sell " + gemScript.upgradeName + " for " + gemScript.upgradePrice / 2 + " coins?", () => SellGem(gem), 0));
        equipButton.GetComponentInChildren<Text>().text = "Sell";
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

    private void SellGem(GameObject gem)
    {
        inventory.AddCoins(gem.GetComponent<UpgradeScript>().upgradePrice / 2);
        inventory.RemoveItem(gem);
        OpenSellGemsMenu();
    }

    private void OpenBuyMenuForGem(GameObject gem, string text, UnityEngine.Events.UnityAction action, int coinsNeeded)
    {
        buyWeaponImage.sprite = gem.GetComponent<UpgradeScript>().upgradeSprite;
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

    private void TriggerWeaponSell(GameObject weapon)
    {
        inventory.AddCoins(weapon.GetComponent<WeaponScript>().weaponPrice / 2);
        inventory.RemoveItem(weapon);
        OpenSellWeaponsMenu();
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
        sellGemsPanel.SetActive(false);
        sellWeaponsPanel.SetActive(false);
    }

    public void GoBack()
    {
        buyWeaponsPanel.SetActive(true);
        buyPanel.SetActive(false);
        buyGemsPanel.SetActive(false);
    }

    public void GoBackDescription()
    {
        infoPanel.SetActive(false);
    }

    private void GoBackAfterGemPurchase()
    {
        buyPanel.SetActive(false);
    }

    private void ReloadGems(RectTransform contentHolder, List<GameObject> gemsList, UnityEngine.Events.UnityAction<Button, Button, string, GameObject> onClickAdder)
    {
        foreach (GameObject row in gemRows)
        {
            Destroy(row);
        }
        ListGems(contentHolder, gemsList, onClickAdder);
    }

    private void ReloadWeapons(RectTransform contentHolder, List<GameObject> weaponList, UnityEngine.Events.UnityAction<GameObject, Button> onClickAdder)
    {
        foreach (GameObject row in rows)
        {
            Destroy(row);
        }
        ListAllWeapons(contentHolder, weaponList, onClickAdder);
    }
}
