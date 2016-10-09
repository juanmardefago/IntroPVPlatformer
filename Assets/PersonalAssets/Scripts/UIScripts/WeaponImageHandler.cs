using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponImageHandler : MonoBehaviour
{

    public Inventory playerInventory;
    private Image img;

    public void Awake()
    {
        img = gameObject.GetComponent<Image>();
    }

    // Use this for initialization
    void Start()
    {
        RefreshWeaponImage();
    }

    public void RefreshWeaponImage()
    {
        WeaponScript weapon = playerInventory.currentWeapon;
        if (weapon != null)
        {
            img.color = new Color32(255, 255, 255, 255);
            img.sprite = weapon.gameObject.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            img.color = new Color32(255, 255, 255, 0);
        }
    }

}
