using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BulletsHandler : MonoBehaviour {

    public GameObject player;
    private WeaponScript playerWeapon;
    private Text textUI;
    private bool isColorInReloadingState;
    public Color reloadingColor;
    public Color normalColor;

    // Use this for initialization
    void Start()
    {
        playerWeapon = player.GetComponentInChildren<WeaponScript>();
        textUI = gameObject.GetComponent<Text>();
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
        int currentBullets = playerWeapon.Bullets;
        int maxBullets = playerWeapon.maxBullets;
        textUI.text = currentBullets + " / " + maxBullets;
        if (playerWeapon.Reloading())
        {
            ChangeColorToReloading();
        } else
        {
            ChangeColorToNormal();
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
}
