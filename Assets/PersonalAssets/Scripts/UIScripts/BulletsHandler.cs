using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BulletsHandler : MonoBehaviour
{

    public Inventory playerInventory;
    private WeaponScript playerWeapon;
    private Image bulletBar;
    private bool isColorInReloadingState;
    public Color reloadingColor;
    public Color normalColor;

    public void Awake()
    {
        bulletBar = gameObject.GetComponent<Image>();
    }

    // Use this for initialization
    void Start()
    {
        playerWeapon = playerInventory.currentWeapon;
        bulletBar.fillAmount = 1;
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
            bulletBar.fillAmount = (float )currentBullets / maxBullets;
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
            bulletBar.fillAmount = 0;
        }
    }

    private void ChangeColorToReloading()
    {
        if (!isColorInReloadingState)
        {
            bulletBar.color = reloadingColor;
            isColorInReloadingState = true;
        }
    }

    private void ChangeColorToNormal()
    {
        if (isColorInReloadingState)
        {
            bulletBar.color = normalColor;
            isColorInReloadingState = false;
        }
    }

    public void RefreshWeaponBullets()
    {
        playerWeapon = playerInventory.currentWeapon;
    }
}
