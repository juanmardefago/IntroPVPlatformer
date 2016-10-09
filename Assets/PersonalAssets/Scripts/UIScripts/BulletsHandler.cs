using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BulletsHandler : MonoBehaviour
{

    public Inventory playerInventory;
    private WeaponScript playerWeapon;
    private Text textUI;
    private bool isColorInReloadingState;
    public Color reloadingColor;
    public Color normalColor;

    public void Awake()
    {
        textUI = gameObject.GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {
        playerWeapon = playerInventory.currentWeapon;
        textUI.text = "";
        isColorInReloadingState = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleBulletAmount();
    }

    private void HandleBulletAmount()
    {
        if (playerWeapon != null)
        {
            int currentBullets = playerWeapon.Bullets;
            int maxBullets = playerWeapon.maxBullets;
            textUI.text = currentBullets + " / " + maxBullets;
            if (playerWeapon.Reloading())
            {
                ChangeColorToReloading();
            }
            else
            {
                ChangeColorToNormal();
            }
        }
        else
        {
            textUI.text = "";
        }
    }

    private void ChangeColorToReloading()
    {
        if (!isColorInReloadingState)
        {
            textUI.color = reloadingColor;
            isColorInReloadingState = true;
        }
    }

    private void ChangeColorToNormal()
    {
        if (isColorInReloadingState)
        {
            textUI.color = normalColor;
            isColorInReloadingState = false;
        }
    }

    public void RefreshWeaponBullets()
    {
        playerWeapon = playerInventory.currentWeapon;
    }
}
